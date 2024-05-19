using Predictor.Domain.System;

namespace Predictor.Domain.Models;

public class FsmStatefulContainer
{
    public PredictorFsmStates CurrentState { get; set; } = PredictorFsmStates.Weather;
}