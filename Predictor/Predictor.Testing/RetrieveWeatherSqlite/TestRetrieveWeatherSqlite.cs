using Microsoft.Extensions.Configuration;
using Predictor.Domain.Models;
using Predictor.Testing.Supporting;

namespace Predictor.Testing.RetrieveWeatherSqlite
{
    public class TestRetrieveWeatherSqlite
    {
        private readonly IConfiguration _configuration = ConfigurationSingleton.Instance;

        [Theory]
        [InlineData(2024, 6, 19, 15, "Utica")]
        [InlineData(2024, 6, 20, 15, "Utica")]
        public async Task TestRetrieve(int year, int month, int day, int hour, string storeName)
        {
            string? tempDatabaseName = null;
            try
            {
                // Arrange
                var setUpResult = await SqliteWeatherHelpers.SetUpDataBaseWithRecordsWeatherCache("Utica", new DateTime(year, month, day, hour, 0, 0), _configuration);
                tempDatabaseName = setUpResult!.dbFileName;
                var sut = new Predictor.RetrieveOwmWeatherSqlite.Implementations.RetrieveWeather(setUpResult.connString!);

                var (@long, lat) = _configuration.Coordinates(storeName);
                var weatherParams = new WeatherRetrieveParamModel
                {
                    DateTime = new DateTime(year, month, day, hour, 0, 0),
                    Latitude = lat,
                    Longitude = @long
                };

                // Act

                var result = await sut.Retrieve(weatherParams);

                // Assert
                Assert.NotNull(result);
                //Assert.Equal(salesAtThree, result.SalesAtThree, 0);
                //Assert.Equal(firstOrderTime, result.FirstOrderMinutesInDay);
                //Assert.Equal(lastOrderTime, result.LastOrderMinutesInDay);
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
}
