using Predictor.Domain.Models;

namespace Predictor.Domain.Abstractions;

public interface IRetrieveWeather
{
    Task<WeatherSourceModel?> Retrieve(WeatherRetrieveParamModel inParams);
}