using Microsoft.Extensions.Configuration;
using System.Data.SQLite;
using System.Globalization;
using Dapper;

namespace Predictor.Testing.Supporting;

internal class SqliteWeatherHelpers
{
    internal static async Task<(string? connString, string? dbFileName)> SetUpDataBaseWithRecordsWeatherCache(string store, DateTime startDateTime, IConfiguration config)
    {
        // Make a copy of the database file.
        var (connString, dbFileName) = SetUpDataBaseNoRecordsWeatherCache(config);

        // Connect to the new database.
        await using var conn = new SQLiteConnection(connString);

        // Clean first.
        await conn.ExecuteAsync("DELETE FROM Weather;");

        // Determine the longitude and latitude based on the store name.
        var (longitude, latitude) = config.Coordinates(store);

        // Now shove in new data. // TODO - left off here 
        const string queryString = "INSERT INTO Weather " +
                                   "(Longitude, Latitude, DateTime, WeatherJson, InsertedUtcTimeStamp) " +
                                   "VALUES (@Longitude, @Latitude, @DateTime, @WeatherJson, @InsertedUtcTimeStamp)";
        var queryParams = new
        {
            Longitude = longitude,
            Latitude = latitude,
            DateTime = startDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
            WeatherJson = Properties.Resources.WeatherData_05152024,
            InsertedUtcTimeStamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)
        };
        await conn.ExecuteAsync(queryString, queryParams);
        await Task.Delay(50);
            
        return (connString, dbFileName);
    }

    internal static (string? connString, string? dbFileName) SetUpDataBaseNoRecordsWeatherCache(IConfiguration config)
    {
        // Make a copy of the database file.
        var connString = config["ConnectionStringSqliteWeatherCache"]!;
        var split = connString.Split(';');
        var originalFileName = split[0].Split('=')[1];
        var newFileName = Path.Combine(".", $"CACHE_SQLITE_DB_WEATHER_{Guid.NewGuid()}.db");
        File.Copy(originalFileName, newFileName);

        // Connect to the new database.
        var tempConnectionString = connString.Replace(originalFileName, newFileName);

        return (tempConnectionString, newFileName);
    }
}