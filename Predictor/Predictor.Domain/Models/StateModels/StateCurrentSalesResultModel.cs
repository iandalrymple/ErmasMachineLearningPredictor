namespace Predictor.Domain.Models.StateModels;

public sealed class StateCurrentSalesResultModel
{
    public required decimal SalesAtThree { get; init; }
    public required uint FirstOrderMinutesInDay { get; init; }
    public required uint LastOrderMinutesInDay { get; set; }
}