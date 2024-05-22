using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Predictor.Domain.Abstractions;
using Predictor.Domain.Models;
using Predictor.Testing.Resources;

namespace Predictor.Testing.Mock;

internal class MockLoggingDecoratedRetrieveWeather : IRetrieveWeather
{
    public Task<WeatherSourceModel> Retrieve(WeatherRetrieveParamModel inParams)
    {
        var rawData = Properties

        throw new NotImplementedException();
    }
}