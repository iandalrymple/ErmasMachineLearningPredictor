using Microsoft.Extensions.Configuration;
using Predictor.Testing.Supporting;

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
            var setUpResult = await SqliteSalesHelpers.SetUpDataBaseWithRecordsSalesCache("Utica", new DateTime(year, month, day), _configuration, 3);
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
}