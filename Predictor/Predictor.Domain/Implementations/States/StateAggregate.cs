using Predictor.Domain.Abstractions;
using Predictor.Domain.Models;
using Predictor.Domain.System;

namespace Predictor.Domain.Implementations.States;

public class StateAggregate : IFsmState
{
    public StateAggregate()
    {
        State = PredictorFsmStates.Aggregate;
    }
    
    public PredictorFsmStates State { get; init; }

    public Task Execute(FsmStatefulContainer container)
    {
        throw new NotImplementedException();
    }
}