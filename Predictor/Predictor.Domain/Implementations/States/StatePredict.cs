using Predictor.Domain.Abstractions;
using Predictor.Domain.Models;
using Predictor.Domain.System;

namespace Predictor.Domain.Implementations.States;

public class StatePredict : IFsmState
{
    private readonly IPredictingEngine _predictingEngine;

    public StatePredict(IPredictingEngine predictingEngine)
    {
        State = PredictorFsmStates.Predict;
        _predictingEngine = predictingEngine;
    }

    public PredictorFsmStates State { get; init; }
    public async Task Execute(FsmStatefulContainer container)
    {
        var paramModel = new PredictingEngineParameterModel();
        var result = await _predictingEngine.RunProcessAsync(paramModel);
        throw new NotImplementedException();
    }
}