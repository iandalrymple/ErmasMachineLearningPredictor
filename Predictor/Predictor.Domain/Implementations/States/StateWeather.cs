using System.Collections.Concurrent;
using Predictor.Domain.Abstractions;
using Predictor.Domain.Extensions;
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
        var weatherDictionary = new ConcurrentDictionary<int, WeatherSourceModel>();
        weatherDictionary.TryAdd(12, await GetWeatherCertainTime(12, container));
        weatherDictionary.TryAdd(15, await GetWeatherCertainTime(15, container));
        weatherDictionary.TryAdd(18, await GetWeatherCertainTime(18, container));
        weatherDictionary.TryAdd(21, await GetWeatherCertainTime(21, container));
        var resultModel = new StateWeatherResultModel
        {
            WeatherAtTimes = weatherDictionary
        };
        container.StateResults.TryAdd(State, resultModel);
        container.CurrentState++;
    }

    private async Task<WeatherSourceModel> GetWeatherCertainTime(int hour, FsmStatefulContainer container)
    {
        // Get the date locally in order to manipulate with hours and get the unix stamps.
        var localDateTime = container.DateToCheck;

        // Get the weather at noon.
        var specificHour = localDateTime.DateTimeAtCertainHour(hour);
        var paramObject = new WeatherRetrieveParamModel
        {
            Latitude = container.StoreLocation.Latitude,
            Longitude = container.StoreLocation.Longitude,
            DateTime = specificHour
        };
        return await _retriever.Retrieve(paramObject);
    }
}