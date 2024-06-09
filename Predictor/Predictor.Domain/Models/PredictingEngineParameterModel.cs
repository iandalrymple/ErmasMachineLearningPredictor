namespace Predictor.Domain.Models;

public class PredictingEngineParameterModel
{
    public required string StoreName { get; init; }
    public required string Features { get; init; }
}