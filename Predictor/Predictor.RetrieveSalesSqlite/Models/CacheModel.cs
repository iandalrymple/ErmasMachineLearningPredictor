namespace Predictor.RetrieveSalesSqlite.Models
{
    internal class CacheModel
    {
        internal int DbId { get; init; }
        internal decimal SalesThreePm { get; init; }
        internal int FirstOrderMinutesIntoDay { get; init; }
        internal string Store { get; init; } = string.Empty;
        internal string Date { get; init; } = string.Empty;
    }
}
