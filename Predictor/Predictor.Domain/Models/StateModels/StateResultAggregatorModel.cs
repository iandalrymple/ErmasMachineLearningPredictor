﻿namespace Predictor.Domain.Models.StateModels;

public class StateResultAggregatorModel
{
    public StateWeatherResultModel? StateWeatherResults { get; set; }
    public StateSalesResultModel? StateSalesResults { get; set; }
}