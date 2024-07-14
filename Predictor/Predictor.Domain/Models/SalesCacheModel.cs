namespace Predictor.Domain.Models
{
    public class SalesCacheModel
    {
        public int DbId { get; init; }
        public decimal SalesThreePm { get; init; }
        public int FirstOrderMinutesIntoDay { get; init; }
        public string Store { get; init; } = string.Empty;
        public string Date { get; init; } = string.Empty;
    }
}
