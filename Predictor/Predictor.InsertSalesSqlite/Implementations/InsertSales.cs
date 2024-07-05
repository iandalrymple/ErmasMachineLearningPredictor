using Predictor.Domain.Abstractions;
using Predictor.Domain.Models;
using System.Data.SQLite;
using System.Globalization;
using Dapper;

namespace Predictor.InsertSalesSqlite.Implementations;

public class InsertSales : ISalesInsert<CacheModel>
{
    private readonly string _connectionString;
    private readonly SQLiteConnection _connection;

    public InsertSales(string connectionString)
    {
        _connectionString = connectionString;
        _connection = new SQLiteConnection(connectionString);
    }

    public async Task<bool> Insert(CacheModel insertionData)
    {
        await _connection.OpenAsync();
        const string queryString = "INSERT INTO CurrentSales " +
                                   "(SalesThreePm, FirstOrderMinutesIntoDay, Store, Date, InsertedUtcTimeStamp) " +
                                   "VALUES (@SalesThreePm, @FirstOrderMinutesIntoDay, @Store, @Date, @InsertedUtcTimeStamp)";
        var queryParams = new
        {
            insertionData.SalesThreePm,
            insertionData.FirstOrderMinutesIntoDay,
            insertionData.Store,
            insertionData.Date,
            InsertedUtcTimeStamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)
        };
        var result = await _connection.ExecuteAsync(queryString, queryParams);
        await _connection.CloseAsync();

        return result > 0;
    }
}