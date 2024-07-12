using Predictor.Domain.Models.StateModels;
using Predictor.Domain.System;

namespace Predictor.Domain.Models;

public class FsmStatefulContainer
{
    private const string Separator = " || ";

    public PredictorFsmStates CurrentState { get; set; } = PredictorFsmStates.Weather;
    public required StoreLocation StoreLocation { get; init; }
    public required DateTime DateToCheck { get; init; }
    public required StatesCombinedResultModel StateResults { get; init; }

    public ErrorModel? ApplicableError { get; set; }

    public string Log()
    {
        string errorMessage;
        if (ApplicableError is null)
        {
            errorMessage = "No Errors";
        }
        else
        {
            errorMessage = ApplicableError.Exception is null ? 
                $"{ApplicableError.Message}" : 
                $"{ApplicableError.Message} => {ApplicableError.Exception.Message}";
        }

        var predictionMessage = ((StateResults.StatePredictResults is null ) || (StateResults.StatePredictResults.PredictingEngineModel.ParsedModelFromStandardInput is null)) ? 
            "No prediction made" : 
            $"{StateResults.StatePredictResults.PredictingEngineModel.ParsedModelFromStandardInput.Prediction:C}";

        var finalString = $"State => {CurrentState}{Separator}" +
                          $"Store => {StoreLocation.Name}{Separator}" +
                          $"Date => {DateToCheck:MM/dd/yyyy}{Separator}" +
                          $"Results => {predictionMessage}{Separator}" +
                          $"Error => {errorMessage}";

        return finalString;
    }
}