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

    public RetrieveWeather(RestClient inClient, RestRequest inRequest)
    {
        _client = inClient ?? throw new ArgumentNullException(nameof(inClient));
        _request = inRequest ?? throw new ArgumentNullException(nameof(inRequest));
    }

    public async Task<WeatherSourceModel> Retrieve(WeatherRetrieveParamModel inParams)
    {
        // Test exception gobbling.
        //if (Math.Abs(inParams.Latitude - 42.534500122070313) < 0.01)
        //    throw new Exception("Just a test error");

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
        _request.AddOrUpdateParameter("appid", inParams.AppId);

        // Execute the request
        var apiResponse = await _client.GetAsync(_request);

        // Make sure not null
        if (apiResponse.Content == null ||
            JsonConvert.DeserializeObject<WeatherSourceModel>(apiResponse.Content, settings) == null)
            throw new WeatherDataNotFoundException(inParams.DateTime);

        // Deserialize the response - don't care about nulls as the exception above will catch it and describe it.
        return JsonConvert.DeserializeObject<WeatherSourceModel>(apiResponse.Content, settings)!;
    }
}