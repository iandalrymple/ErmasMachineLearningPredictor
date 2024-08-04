using Predictor.Domain.Abstractions;
using Predictor.Domain.Models;
using System.Data.SQLite;
using System.Globalization;
using Dapper;

namespace Predictor.InsertWeatherSqlite.Implementations;

public class InsertWeather : IGenericInsert<WeatherCacheModel>
{
    private readonly string _connectionString;
    private readonly SQLiteConnection _connection;

    public InsertWeather(string connectionString)
    {
        _connectionString = connectionString;
        _connection = new SQLiteConnection(connectionString);
    }

    public async Task<bool> Insert(WeatherCacheModel insertionData)
    {
        await _connection.OpenAsync();
        const string queryString = "INSERT INTO Weather " +
                                   "(Longitude, Latitude, DateTime, WeatherJson, InsertedUtcTimeStamp) " +
                                   "VALUES (@Longitude, @Latitude, @DateTime, @WeatherJson, @InsertedUtcTimeStamp)";
        var queryParams = new
        {
            Longitude = insertionData.Longitude,
            Latitude = insertionData.Latitude,
            DateTime = insertionData.DateTime,
            WeatherJson = insertionData.WeatherJson,
            InsertedUtcTimeStamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)
        };

        var result = await _connection.ExecuteAsync(queryString, queryParams);
        await _connection.CloseAsync();

        return result > 0;
    }
}