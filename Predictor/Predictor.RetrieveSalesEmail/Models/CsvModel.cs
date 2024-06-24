namespace Predictor.RetrieveSalesEmail.Models
{
    internal class CsvModel
    {
        private readonly List<CsvRowModel> _rows = [];

        internal CsvModel(string csv)
        {
            foreach (var row in csv.Split(Environment.NewLine))
            {
                if (string.IsNullOrEmpty(row))
                {
                    continue;
                }
                _rows.Add(new CsvRowModel(row));
            }

            FirstOrderInMinutesFromStartOfDay = ProcessFirstOrder(_rows);
            SalesAtThree = ProcessSalesAtThree(_rows);
        }

        internal uint FirstOrderInMinutesFromStartOfDay { get; init; }
        internal decimal SalesAtThree { get; init; }

        private static uint ProcessFirstOrder(List<CsvRowModel> rows)
        {
            if (rows.Count == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(rows));
            var firstOrder = rows
                .OrderBy(r => r.StartTime)
                .First(r => r.TotalChecksTimePeriod > 0);
            var firstOrderTime = firstOrder.StartTime;
            var totalMinutes = Convert.ToUInt32(firstOrderTime.Hour * 60 + firstOrderTime.Minute);
            return totalMinutes;
        }

        private static decimal ProcessSalesAtThree(List<CsvRowModel> rows)
        {
            if (rows.Count == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(rows));
            var threeRow = rows.
                First(r => r.EndTime == new TimeOnly(hour: 15, minute: 0, second: 0));
            return threeRow.TotalSalesCumulative;
        }
    }
}
