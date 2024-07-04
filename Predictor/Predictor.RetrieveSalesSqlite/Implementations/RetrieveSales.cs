using System.Data.SQLite;
using Dapper;
using Predictor.Domain.Abstractions;
using Predictor.Domain.Exceptions;
using Predictor.Domain.Models.StateModels;
using Predictor.Domain.Models;

namespace Predictor.RetrieveSalesSqlite.Implementations
{
    public class RetrieveSales(string connectionString) : IRetrieveSales<StateCurrentSalesResultModel?>
    {
        // Note:    May want to reuse the connection. At this time it's not required since the
        //          hit rate will be very low. So we are leaving as is.

        public async Task<StateCurrentSalesResultModel?> Retrieve(DateTime dateTime, string storeName)
        {
            await using var conn = new SQLiteConnection(connectionString);
            const string queryString = "SELECT * FROM CurrentSales WHERE Store=@Store AND [Date]=@Date;";
            var queryParams = new
            {
                Store = storeName,
                Date = dateTime.ToString("yyyy-MM-dd")
            };

            var result = (await conn.QueryAsync<CacheModel>(queryString, queryParams)).ToList();

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
