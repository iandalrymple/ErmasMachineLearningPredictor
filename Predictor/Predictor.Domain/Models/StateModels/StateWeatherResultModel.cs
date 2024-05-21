using System.Collections.Concurrent;

namespace Predictor.Domain.Models.StateModels;

public class StateWeatherResultModel
{
    public required ConcurrentDictionary<int, WeatherSourceModel> WeatherAtTimes { get; init; }

}