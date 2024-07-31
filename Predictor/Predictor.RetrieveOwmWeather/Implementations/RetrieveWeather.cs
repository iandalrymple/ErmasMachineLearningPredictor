using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Predictor.Domain.Abstractions;
using Predictor.Domain.Exceptions;
using Predictor.Domain.Models;
using RestSharp;

namespace Predictor.RetrieveOwmWeather.Implementations;

public class RetrieveWeather : IRetrieveWeather
{
    // Backing 
    private readonly RestClient _client;
    private readonly RestRequest _request;
    private readonly string _appId;

    private readonly ILogger<RetrieveWeather> _logger;

    private readonly IRetrieveWeather _cacheRetriever;

    //Need the Sqlite implementation injected (Retrieve and insert)
    // Use them both just like in the sales retriever

    public RetrieveWeather(string baseUri, string appId, ILogger<RetrieveWeather> logger, IRetrieveWeather cacheRetriever)
    {
        var rco = new RestClientOptions(baseUri);
        _client = new RestClient(rco);
        _request = new RestRequest();
        _appId = appId;

        _logger = logger;

        _cacheRetriever = cacheRetriever;
    }

    public async Task<WeatherSourceModel?> Retrieve(WeatherRetrieveParamModel inParams)
    {
        // Test exception gobbling.
        //if (Math.Abs(inParams.Latitude - 42.534500122070313) < 0.01)
        //    throw new Exception("Just a test error");

        // First just check if it's available in the cache. 
        var cacheCheck = await CheckCache(inParams);
        if (cacheCheck is not null)
        {
            return cacheCheck;
        }

        // Create some settings for null handling 
        // https://stackoverflow.com/questions/31813055/how-to-handle-null-empty-values-in-jsonconvert-deserializeobject
        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        // Add the parameters
        _request.AddOrUpdateParameter("dt", ((DateTimeOffset)inParams.DateTime).ToUnixTimeSeconds());
        _request.AddOrUpdateParameter("lat", inParams.Latitude);
        _request.AddOrUpdateParameter("lon", inParams.Longitude);
        _request.AddOrUpdateParameter("appid", _appId);

        // Execute the request
        var apiResponse = await _client.GetAsync(_request);

        // Make sure not null
        if (apiResponse.Content == null ||
            JsonConvert.DeserializeObject<WeatherSourceModel>(apiResponse.Content, settings) == null)
            throw new WeatherDataNotFoundException(inParams.DateTime);

        // Toss a log for showing what has happened.
        _logger.LogInformation("Retrieved the following Datetime: {Dt} Latitude: {Lat} Longitude {Lon}", inParams.DateTime, inParams.Latitude, inParams.Longitude);

        // Deserialize the response - don't care about nulls as the exception above will catch it and describe it.
        return JsonConvert.DeserializeObject<WeatherSourceModel>(apiResponse.Content, settings)!;
    }

    private async Task<WeatherSourceModel?> CheckCache(WeatherRetrieveParamModel inParams)
    {
        WeatherSourceModel? cacheResult = null;

        try
        {
            cacheResult = await _cacheRetriever.Retrieve(inParams);
        }
        catch (Exception ex)
        {
            // Intentionally logging and NOT throwing.
            _logger.LogWarning("Error checking weather cache with {exception}", ex);
        }

        if (cacheResult is not null)
        {
            _logger.LogInformation("Cache HIT for {dateTime} and {latitude}/{longitude}.", inParams.DateTime.ToString("MM/dd/yyyy"), inParams.Latitude, inParams.Longitude);
        }
        else
        {
            _logger.LogInformation("Cache HIT for {dateTime} and {latitude}/{longitude}.", inParams.DateTime.ToString("MM/dd/yyyy"), inParams.Latitude, inParams.Longitude);
        }

        return cacheResult;
    }
}