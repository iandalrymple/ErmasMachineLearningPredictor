using System.Collections.Concurrent;
using Predictor.Domain.System;

namespace Predictor.Domain.Models;

public class FsmStatefulContainer
{
    public PredictorFsmStates CurrentState { get; set; } = PredictorFsmStates.Weather;
    public required StoreLocation StoreLocation { get; init; }
    public required DateTime DateToCheck { get; init; }

    public required ConcurrentDictionary<PredictorFsmStates, object> StateResults { get; init; }
}