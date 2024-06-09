using Predictor.Domain.Models;

namespace Predictor.Domain.Abstractions;

public interface IPredictingEngine
{
    Task<PredictingEngineResponseModel> PredictAsync(PredictingEngineParameterModel parameterModel);
}