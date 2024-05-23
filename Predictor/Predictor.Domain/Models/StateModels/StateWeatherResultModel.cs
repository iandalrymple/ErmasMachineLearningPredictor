using System.Collections.Concurrent;

namespace Predictor.Domain.Models.StateModels;

public sealed class StateWeatherResultModel
{
    public required ConcurrentDictionary<int, WeatherSourceModel> WeatherAtTimes { get; init; }

}