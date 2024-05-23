namespace Predictor.Domain.Models.StateModels;

public sealed class StateHistoricSalesResultModel
{
    public required decimal SalesDayBefore { get; init; }
    public required decimal SalesTwoDaysBefore { get; init; }
}