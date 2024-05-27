using Predictor.Domain.Abstractions;
using System.Data.SqlClient;
using Dapper;
using Predictor.RetrieveSalesSqlServer.Models;

namespace Predictor.RetrieveSalesSqlServer.Implementations;

public class RetrieveSales : IRetrieveSales<decimal>
{
    private readonly string _connectionString;

    public RetrieveSales(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public async Task<decimal> Retrieve(DateTime dateTime, string storeName)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        var sqlQuery = "SELECT * FROM viewPaymentsByStoreByDay " +
                       "WHERE StoreName = @StoreName " +
                       "AND [Year] = @Year " +
                       "AND [Month] = @Month " +
                       "AND [Day] = @Day;";
        var result = await connection.QueryAsync<ViewPaymentsByStoreByDayResultModel>
        (
            sqlQuery,
            new
            {
                StoreName = storeName,
                dateTime.Year,
                dateTime.Month,
                dateTime.Day
            }
        );
        return Convert.ToDecimal(result.First().TotalSales);
    }
}