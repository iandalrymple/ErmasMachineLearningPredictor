namespace Predictor.Domain.System;

public enum PredictorFsmStates
{
    First,

    Weather,
    CurrentSalesRetrieve, 
    HistoricSalesRetrieve,
    Aggregate,
    Predict,

    Completed,
    Error,

    Last
}