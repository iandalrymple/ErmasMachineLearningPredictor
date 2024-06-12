using Predictor.Domain.Abstractions;
using Predictor.Domain.Models;
using Predictor.Domain.Models.StateModels;
using Predictor.Domain.System;

namespace Predictor.Domain.Implementations.States;

public class StatePredict : IFsmState
{
    private readonly IPredictingEngine _predictingEngine;

    public StatePredict(IPredictingEngine predictingEngine)
    {
        State = PredictorFsmStates.Predict;
        _predictingEngine = predictingEngine;
    }

    public PredictorFsmStates State { get; init; }
    public async Task Execute(FsmStatefulContainer container)
    {
        // Check if the container elements are null.
        if (container.StateResults.StateAggregateResults is null)
        {
            container.CurrentState = PredictorFsmStates.Error;
            container.ApplicableError = new ErrorModel
            {
               Message = $"{nameof(container.StateResults.StateAggregateResults)} was null.",
               StateErrorOccurredIn = State,
               Exception = null
            };
            return;
        }

        // Get the csv of features and check if null.
        var rawFeatureString = container.StateResults.StateAggregateResults.CreateCsvRows();
        if (string.IsNullOrEmpty(rawFeatureString))
        {
            container.CurrentState = PredictorFsmStates.Error;
            container.ApplicableError = new ErrorModel
            {
                Message = "Features CSV was null.",
                StateErrorOccurredIn = State,
                Exception = null
            };
            return;
        }

        // Spin up a temp file to put on the input params.
        var fileName = await PredictingEngineParameterModel.CreateTempFile(rawFeatureString);
        var inputParams = new PredictingEngineParameterModel { StoreName = container.StoreLocation.Name, FeaturesPath = fileName };

        // Finally call the predictor.
        var result = await _predictingEngine.PredictAsync(inputParams);

        // Stuff the results into the container.
        container.StateResults.StatePredictResults = new StatePredictResultModel
        {
            PredictingEngineModel = result
        };

        // Advance the state. 
        container.CurrentState++;
    }
}