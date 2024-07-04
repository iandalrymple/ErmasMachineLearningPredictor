using Dapper;
using Microsoft.Extensions.Configuration;
using Predictor.Testing.Supporting;
using System.Data.SQLite;
using System.Globalization;

namespace Predictor.Testing.RetrieveSalesSqlite;

public class TestRetrieveSalesSqlite
{
    private readonly IConfiguration _configuration = ConfigurationSingleton.Instance;

    [Theory]
    [InlineData(2024, 6, 19, 1000.00, 650, uint.MaxValue)]
    [InlineData(2024, 6, 20, 1000.00, 650, uint.MaxValue)]
    public async Task TestRetrieve(int year, int month, int day, decimal salesAtThree, uint firstOrderTime, uint lastOrderTime)
    {
        string? tempDatabaseName = null;
        try
        {
            // Arrange
            var setUpResult = await SetUpDataBase("Utica", new DateTime(year, month, day), _configuration, 3);
            tempDatabaseName = setUpResult!.dbFileName;
            var sut = new Predictor.RetrieveSalesSqlite.Implementations.RetrieveSales(setUpResult.connString!);

            // Act
            var dateTime = new DateTime(year: year, month: month, day: day);
            var result = await sut.Retrieve(dateTime, "Utica");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(salesAtThree, result.SalesAtThree, 0);
            Assert.Equal(firstOrderTime, result.FirstOrderMinutesInDay);
            Assert.Equal(lastOrderTime, result.LastOrderMinutesInDay);
        }
        finally
        {
            if (!string.IsNullOrEmpty(tempDatabaseName) && File.Exists(tempDatabaseName))
            {
                File.Delete(tempDatabaseName);
            }
        }
    }

    internal static async Task<(string? connString, string? dbFileName)> SetUpDataBase(string store, DateTime startDate, IConfiguration config, int recordCount = 1)
    {
        // Make a copy of the database file.
        var connString = config["ConnectionStringSqlite"]!;
        var split = connString.Split(';');
        var originalFileName = split[0].Split('=')[1];
        var newFileName = Path.Combine(".", $"CACHE_SQLITE_DB_{Guid.NewGuid()}.db");
        File.Copy(originalFileName, newFileName);
        
        // Connect to the new database.
        var tempConnectionString = connString.Replace(originalFileName, newFileName);
        await using var conn = new SQLiteConnection(tempConnectionString);

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

        return (tempConnectionString, newFileName);
    }
}