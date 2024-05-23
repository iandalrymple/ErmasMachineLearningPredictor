using Predictor.Domain.Implementations.States;

namespace Predictor.Domain.Models.StateModels;

public class StatesCombinedResultModel
{
    public StateWeatherResultModel? StateWeatherResults { get; set; }
    public StateSalesResultModel? StateSalesResults { get; set; }
    public StateAggregateResultModel? StateAggregateResults { get; set; }
}