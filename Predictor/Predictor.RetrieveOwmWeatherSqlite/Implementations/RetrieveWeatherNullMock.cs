using Predictor.Domain.Abstractions;
using Predictor.Domain.Models;

namespace Predictor.RetrieveOwmWeatherSqlite.Implementations;

public class RetrieveWeatherNullMock : IRetrieveWeather
{
    public async Task<WeatherSourceModel?> Retrieve(WeatherRetrieveParamModel inParams)
    {
        await Task.Delay(100);
        return null;
    }
}