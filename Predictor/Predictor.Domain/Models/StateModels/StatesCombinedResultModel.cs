namespace Predictor.Domain.Models.StateModels;

public sealed class StatesCombinedResultModel
{
    public StateWeatherResultModel? StateWeatherResults { get; set; }
    public StateCurrentSalesResultModel? StateCurrentSalesResults { get; set; }
    public StateHistoricSalesResultModel? StateHistoricSalesResults { get; set; }
    public StateAggregateResultModel? StateAggregateResults { get; set; }
}