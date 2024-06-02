using Predictor.Domain.Abstractions;
using Predictor.Domain.Models;
using Predictor.Domain.System;

namespace Predictor.Domain.Implementations.States;

public class StatePredict : IFsmState
{
    public StatePredict()
    {
        State = PredictorFsmStates.Predict;
    }

    public PredictorFsmStates State { get; init; }
    public Task Execute(FsmStatefulContainer container)
    {
        throw new NotImplementedException();
    }
}