namespace Predictor.RetrieveSalesSqlServer.Models;

internal class ViewPaymentsByStoreByDayResultModel
{
    public string StoreName { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Month { get; set; }
    public int Day { get; set; }
    public decimal TotalSales { get; set; }
    public int OrderCount { get; set; }
}