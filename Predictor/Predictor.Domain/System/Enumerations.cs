namespace Predictor.Domain.System;

public enum PredictorFsmStates
{
    First,

    GetWeather,
    GetSales, 
    KickOffPredictionProcess,

    Completed,
    Error,

    Last
}