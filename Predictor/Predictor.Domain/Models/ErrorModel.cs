using Predictor.Domain.System;

namespace Predictor.Domain.Models;

public class ErrorModel
{
    public required string Message { get; init; }
    public required PredictorFsmStates StateErrorOccurredIn { get; init; }
    public required Exception? Exception { get; init; }
}