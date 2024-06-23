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

            // TODO - need the logic here to get the fields 
        }

        internal uint FirstOrderInMinutesFromStartOfDay { get; init; }
        internal decimal SalesAtThree { get; init; }
    }
}
