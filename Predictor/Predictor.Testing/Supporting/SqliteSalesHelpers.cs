using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SQLite;
using System.Globalization;

namespace Predictor.Testing.Supporting
{
    internal class SqliteSalesHelpers
    {
        internal static async Task<(string? connString, string? dbFileName)> SetUpDataBaseWithRecordsSalesCache(string store, DateTime startDate, IConfiguration config, int recordCount = 1)
        {
            // Make a copy of the database file.
            var (connString, dbFileName) = SetUpDataBaseNoRecordsSalesCache(config);

            // Connect to the new database.
            await using var conn = new SQLiteConnection(connString);

            // Clean first.
            await conn.ExecuteAsync("DELETE FROM CurrentSales;");

            // Now shove in new data.
            const string queryString = "INSERT INTO CurrentSales " +
                                       "(SalesThreePm, FirstOrderMinutesIntoDay, Store, Date, InsertedUtcTimeStamp) " +
                                       "VALUES (@SalesThreePm, @FirstOrderMinutesIntoDay, @Store, @Date, @InsertedUtcTimeStamp)";
            for (var i = 0; i < recordCount; i++)
            {
                var queryParams = new
                {
                    SalesThreePm = Convert.ToDecimal((i + 1) * 1000 + i),
                    FirstOrderMinutesIntoDay = 650 + i,
                    Store = store,
                    Date = startDate.AddDays(i).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    InsertedUtcTimeStamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)
                };
                await conn.ExecuteAsync(queryString, queryParams);
                await Task.Delay(50);
            }

            return (connString, dbFileName);
        }

        internal static (string? connString, string? dbFileName) SetUpDataBaseNoRecordsSalesCache(IConfiguration config)
        {
            // Make a copy of the database file.
            var connString = config["ConnectionStringSqliteSalesCache"]!;
            var split = connString.Split(';');
            var originalFileName = split[0].Split('=')[1];
            var newFileName = Path.Combine(".", $"CACHE_SQLITE_DB_SALES_{Guid.NewGuid()}.db");
            File.Copy(originalFileName, newFileName);

            // Connect to the new database.
            var tempConnectionString = connString.Replace(originalFileName, newFileName);

            return (tempConnectionString, newFileName);
        }
    }
}
