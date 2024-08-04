using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection.Metadata;
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
    }

    public PredictorFsmStates State { get; init; }

    public async Task Execute(FsmStatefulContainer container)
    {
        var weatherDictionary = new ConcurrentDictionary<int, WeatherSourceModel>();
        if (!await HandleWeatherResultInsertion(weatherDictionary, 12, container))
        {
            return;
        }
        if (!await HandleWeatherResultInsertion(weatherDictionary, 15, container))
        {
            return;
        }
        if (!await HandleWeatherResultInsertion(weatherDictionary, 18, container))
        {
            return;
        }
        if (!await HandleWeatherResultInsertion(weatherDictionary, 21, container))
        {
            return;
        }
        var resultModel = new StateWeatherResultModel
        {
            WeatherAtTimes = weatherDictionary
        };
        container.StateResults.StateWeatherResults = resultModel;
        container.CurrentState++;
    }

    private async Task<bool> HandleWeatherResultInsertion(ConcurrentDictionary<int, WeatherSourceModel> dictionary, int hour, FsmStatefulContainer container)
    {
        var recurringResult = await GetWeatherCertainTime(12, container);
        if (recurringResult == null)
        {
            container.ApplicableError = new ErrorModel
            {
                Exception = null,
                Message = "Weather data could not be retrieved.",
                StateErrorOccurredIn = container.CurrentState
            };
            container.CurrentState = PredictorFsmStates.Error;
            return false;
        }
        dictionary.TryAdd(hour, recurringResult);
        return true;    
    }

    private async Task<WeatherSourceModel?> GetWeatherCertainTime(int hour, FsmStatefulContainer container)
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