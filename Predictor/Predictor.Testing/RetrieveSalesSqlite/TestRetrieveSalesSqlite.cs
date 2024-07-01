using System.Runtime.InteropServices.JavaScript;
using Microsoft.Extensions.Configuration;
using Predictor.Testing.Supporting;

namespace Predictor.Testing.RetrieveSalesSqlite;

public class TestRetrieveSalesSqlite
{
    private readonly IConfiguration _configuration = ConfigurationSingleton.Instance;
    private readonly string _temporaryTestingConnString;

    public TestRetrieveSalesSqlite()
    {
        // Make a copy of the database file.
        var connString = _configuration["ConnectionStringSqlite"]!;
        var split = connString.Split(';');
        var originalFileName = split[0].Split('=')[1];
        var newFileName = Path.Combine(".", $"{Guid.NewGuid()}.db");
        File.Copy(originalFileName, newFileName);

        // Need to construct the new connection string.
        _temporaryTestingConnString = connString.Replace(originalFileName, newFileName);
    }

    [Theory]
    [InlineData(2024, 6, 19, 1127.85, 660, 0)]
    [InlineData(2024, 6, 20, 858.53, 660, 0)]
    public async Task TestRetrieve(int year, int month, int day, decimal salesAtThree, uint firstOrderTime, uint lastOrderTime)
    {
        // Arrange
        var sut = new Predictor.RetrieveSalesSqlite.Implementations.RetrieveSales(_temporaryTestingConnString);

        // Act
        var dateTime = new DateTime(year: year, month: month, day: day);
        var result = await sut.Retrieve(dateTime, "Utica");

        // Assert
        Assert.Equal(salesAtThree, result.SalesAtThree, 0);
        Assert.Equal(firstOrderTime, result.FirstOrderMinutesInDay);
        Assert.Equal(lastOrderTime, result.LastOrderMinutesInDay);
    }
}