using Predictor.Domain.Abstractions;
using Predictor.Domain.Models;

namespace Predictor.Testing.Mock;

internal class MockLoggingDecoratedRetrieveWeather : IRetrieveWeather
{
    public Task<WeatherSourceModel> Retrieve(WeatherRetrieveParamModel inParams)
    {
        var rawData = Properties.Resources.WeatherData_05152024;

        throw new NotImplementedException();
    }
}