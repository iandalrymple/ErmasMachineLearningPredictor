using Predictor.Domain.Models;

namespace Predictor.Domain.Abstractions;

public interface IPredictingEngine
{
    Task<PredictingEngineResponseModel> RunProcessAsync(PredictingEngineParameterModel parameterModel);
}