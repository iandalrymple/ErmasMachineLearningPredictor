namespace Predictor.Domain.System;

public enum PredictorFsmStates
{
    First,

    Weather,
    Sales, 
    Predict,

    Completed,
    Error,

    Last
}