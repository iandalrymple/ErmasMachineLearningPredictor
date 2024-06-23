namespace Predictor.RetrieveSalesEmail.Models;

internal class CsvRowModel
{
    // NOTE:    The total checks field is broken and just appears to be the 
    //          same as the time period count. FOCUS issue. Leaving the 
    //          commented out code in for posterity.

    private const int StartTimeIndex = 0;
    private const int EndTimeIndex = 1;
    private const int TotalChecksTimePeriodIndex = 2;
    // private const int TotalChecksCumulativeIndex = 3;
    private const int TotalSalesTimePeriodIndex = 4;
    private const int TotalSalesCumulativeIndex = 5;

    internal CsvRowModel(string row)
    {
        var split = row.Split(',');
        StartTime = split[StartTimeIndex].Trim('"');
        EndTime = split[EndTimeIndex].Trim('"');
        TotalChecksTimePeriod = Convert.ToInt32(split[TotalChecksTimePeriodIndex].Trim('"'));
        // TotalChecksCumulative = Convert.ToInt32(split[TotalChecksCumulativeIndex]);
        TotalSalesTimePeriod = Convert.ToDecimal(split[TotalSalesTimePeriodIndex].Trim('"'));
        TotalSalesCumulative = Convert.ToDecimal(split[TotalSalesCumulativeIndex].Trim('"'));
    }

    internal string StartTime { get; init; }
    internal string EndTime { get; init; }
    internal int TotalChecksTimePeriod { get; init; }

    // public int TotalChecksCumulative { get; init; }
    internal decimal TotalSalesTimePeriod { get; init; }
    internal decimal TotalSalesCumulative { get; init; }
}