using Predictor.Domain.Abstractions;
using Predictor.Domain.Models;
using Predictor.Domain.Models.StateModels;
using Predictor.Domain.System;

namespace Predictor.Domain.Implementations.States;

public class StateWeather : IFsmState
{
    private readonly IRetrieveWeather _retriever;

    public StateWeather(IRetrieveWeather retriever)
    {
        _retriever = retriever; 

        State = PredictorFsmStates.Weather;
        ResultType = typeof(StateWeatherResultModel);
    }

    public PredictorFsmStates State { get; init; }
    public Type ResultType { get; init; }

    public async Task Execute(FsmStatefulContainer container)
    {
        // Get the date locally in order to manipulate with hours and get the unix stamps.
        var localDateTime = container.DateToCheck;

        // Get the weather at noon.
        var timeAtNoon = new DateTime(
            year: localDateTime.Year, 
            month: localDateTime.Month, 
            day: localDateTime.Day, 
            hour: 12, 
            minute: 0,
            second: 0);



        await Task.Delay(1);

        // Get the weather at 3.


        // Get forecast at 6.

        // Get forecast at 9.
    }
}