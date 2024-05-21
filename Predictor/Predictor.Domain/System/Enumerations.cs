namespace Predictor.Domain.System;

public enum PredictorFsmStates
{
    First,

    Weather,
    SalesRetrieve, 
    Aggregate,
    Predict,

    Completed,
    Error,

    Last
}