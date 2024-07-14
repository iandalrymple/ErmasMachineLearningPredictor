using Predictor.Domain.Abstractions;
using Predictor.Domain.Models;

namespace Predictor.RetrieveOwmWeather.Implementations;

public class RetrieveWeatherMock : IRetrieveWeather
{
    public async Task<WeatherSourceModel?> Retrieve(WeatherRetrieveParamModel inParams)
    {
        await Task.Delay(100);
        return new WeatherSourceModel();
    }
}