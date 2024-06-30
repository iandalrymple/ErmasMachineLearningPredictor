using Microsoft.Extensions.Configuration;
using Predictor.Testing.Supporting;

namespace Predictor.Testing.RetrieveSalesSqlite;

public class TestRetrieveSalesSqlite
{
    private readonly IConfiguration _configuration = ConfigurationSingleton.Instance;

    public TestRetrieveSalesSqlite()
    {
        // Make a copy of the database file.
        var connString = _configuration["ConnectionStringSqlite"]!;

    }

    [Theory]
    [InlineData(2024, 6, 19, 1127.85, 660, 0)]
    [InlineData(2024, 6, 20, 858.53, 660, 0)]
    public async Task TestRetrieve(int year, int month, int day, decimal salesAtThree, uint firstOrderTime, uint lastOrderTime)
    {
        // Arrange
        var sut = new Predictor.RetrieveSalesSqlite.Implementations.RetrieveSales(_configuration["ConnectionStringSqlite"]!);

        // Act
        var dateTime = new DateTime(year: year, month: month, day: day);
        var result = await sut.Retrieve(dateTime, "Utica");

        // Assert
        Assert.Equal(salesAtThree, result.SalesAtThree, 0);
        Assert.Equal(firstOrderTime, result.FirstOrderMinutesInDay);
        Assert.Equal(lastOrderTime, result.LastOrderMinutesInDay);
    }
}