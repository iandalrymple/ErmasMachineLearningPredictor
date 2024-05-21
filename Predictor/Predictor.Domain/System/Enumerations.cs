namespace Predictor.Domain.System;

public enum PredictorFsmStates
{
    First,

    Weather,
    SalesRetrieve, 
    Predict,

    Completed,
    Error,

    Last
}