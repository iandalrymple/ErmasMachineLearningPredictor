using System.Data.SQLite;
using Dapper;
using Predictor.Domain.Abstractions;
using Predictor.Domain.Models.StateModels;
using Predictor.RetrieveSalesSqlite.Models;

namespace Predictor.RetrieveSalesSqlite.Implementations
{
    public class RetrieveSales : IRetrieveSales<StateCurrentSalesResultModel?>
    {
        private readonly string _connectionString;

        public RetrieveSales(string connectionString)
        {
            _connectionString = connectionString;   
        }

        public async Task<StateCurrentSalesResultModel?> Retrieve(DateTime dateTime, string storeName)
        {
            await using var conn = new SQLiteConnection(_connectionString);
            var result = await conn.QueryAsync<CacheModel>("SELECT * FROM CurrentSales;", new DynamicParameters());
            return null;
        }
    }
}
