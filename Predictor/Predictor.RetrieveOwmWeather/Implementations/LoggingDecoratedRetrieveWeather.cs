using Predictor.Domain.Abstractions;
using Predictor.Domain.Models;

using Microsoft.Extensions.Logging;

namespace Predictor.RetrieveOwmWeather.Implementations;

public class LoggingDecoratorRetrieveWeather : IRetrieveWeather
{
    private readonly IRetrieveWeather _decoratedRetrieveWeather;
    private readonly ILogger _logger;

    public LoggingDecoratorRetrieveWeather(IRetrieveWeather decoratedRetrieveWeather, ILogger logger)
    {
        _decoratedRetrieveWeather = decoratedRetrieveWeather;
        _logger = logger;
    }

    public async Task<WeatherSourceModel> Retrieve(WeatherRetrieveParamModel inParams)
    {
        try
        {
            // This is a trace log on purpose, so we can turn up the logging verbosity via seq to get this when a problem occurs.
            _logger.LogTrace("Retrieving the following Datetime: {Dt} Latitude: {Lat} Longitude {Lon}", inParams.DateTime, inParams.Latitude, inParams.Longitude);

            return await _decoratedRetrieveWeather.Retrieve(inParams);
        }
        catch (Exception e)
        {
            _logger.LogError("{Ex}", e);
            throw;
        }
    }
}