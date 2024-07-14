using System.Data.SQLite;
using Dapper;
using Predictor.Domain.Abstractions;
using Predictor.Domain.Exceptions;
using Predictor.Domain.Models.StateModels;
using Predictor.Domain.Models;

namespace Predictor.RetrieveSalesSqlite.Implementations
{
    public class RetrieveSales : IRetrieveSales<StateCurrentSalesResultModel?>
    {
        private readonly string _connectionString;
        private readonly SQLiteConnection _connection;

        public RetrieveSales(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new SQLiteConnection(connectionString);
        }

        public async Task<StateCurrentSalesResultModel?> Retrieve(DateTime dateTime, string storeName)
        {
            await _connection.OpenAsync();
            const string queryString = "SELECT * FROM CurrentSales WHERE Store=@Store AND [Date]=@Date;";
            var queryParams = new
            {
                Store = storeName,
                Date = dateTime.ToString("yyyy-MM-dd")
            };

            var result = (await _connection.QueryAsync<SalesCacheModel>(queryString, queryParams)).ToList();
            await _connection.CloseAsync();

            if (result.Count == 0)
            {
                return null;
            }
            if (result.Count > 1)
            {
                throw new MoreThanOneRecordException($"Store => {storeName} / Date => {dateTime}");
            }

            var firstRecord = result[0];
            var returnModel = new StateCurrentSalesResultModel
            {
                FirstOrderMinutesInDay = Convert.ToUInt32(firstRecord.FirstOrderMinutesIntoDay),
                LastOrderMinutesInDay = uint.MaxValue,
                SalesAtThree = firstRecord.SalesThreePm
            };
            return returnModel;
        }
    }
}
