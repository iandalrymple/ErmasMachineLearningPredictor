using Predictor.Domain.Abstractions;
using Predictor.Domain.Models;
using Predictor.Domain.System;

namespace Predictor.Domain.Implementations.States;

public class StateWeather : IFsmState
{
    private readonly IRetrieveWeather _retriever;

    public StateWeather(IRetrieveWeather retriever)
    {
        _retriever = retriever; 

        State = PredictorFsmStates.Weather;
    }

    public PredictorFsmStates State { get; init; }

    public Task Execute(FsmStatefulContainer container)
    {
        throw new NotImplementedException();
    }
}