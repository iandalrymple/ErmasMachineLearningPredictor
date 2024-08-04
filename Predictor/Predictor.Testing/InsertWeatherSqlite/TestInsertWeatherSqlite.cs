using Microsoft.Extensions.Configuration;
using Predictor.Domain.Models;
using Predictor.Testing.Supporting;
using System.Globalization;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Predictor.Testing.InsertWeatherSqlite;

public class TestInsertWeatherSqlite
{
    private readonly IConfiguration _configuration = ConfigurationSingleton.Instance;

    [Fact]
    public async Task TestInsert()
    {
        string? tempDatabaseName = null;
        try
        {
            // Arrange
            const string storeName = "Utica"; 
            var setUpResult = await SqliteWeatherHelpers.SetUpDataBaseWithRecordsWeatherCache(storeName, new DateTime(2024, 1, 1), _configuration);
            tempDatabaseName = setUpResult!.dbFileName;
            var sut = new Predictor.InsertWeatherSqlite.Implementations.InsertWeather(setUpResult.connString!);
            var (longitude, latitude) = _configuration.Coordinates(storeName);
            var insertionDataOne = new WeatherCacheModel
            {
                Longitude = longitude,
                Latitude = latitude,
                DateTime = DateTime.UtcNow.Subtract(TimeSpan.FromDays(-5)).ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture),
                WeatherJson = Properties.Resources.WeatherApiData_04192023_12AM,
            };

            // Act
            var resultOne = await sut.Insert(insertionDataOne);

            // Assert
            Assert.True(resultOne);
        }
        finally
        {
            if (!string.IsNullOrEmpty(tempDatabaseName) && File.Exists(tempDatabaseName))
            {
                File.Delete(tempDatabaseName);
            }
        }
    }
}