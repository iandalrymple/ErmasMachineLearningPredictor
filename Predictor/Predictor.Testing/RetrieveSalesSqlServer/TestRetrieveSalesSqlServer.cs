using Microsoft.Extensions.Configuration;
using Predictor.Testing.Supporting;

namespace Predictor.Testing.RetrieveSalesSqlServer;

public class TestRetrieveSalesSqlServer
{
    private readonly IConfiguration _configuration;

    private readonly DateTime _dateToRetrieve;

    public TestRetrieveSalesSqlServer()
    {
        _configuration = ConfigurationSingleton.Instance;
        _dateToRetrieve = DateTime.SpecifyKind(new DateTime(year: 2023, month: 6, day: 19), DateTimeKind.Utc);
    }

    [Fact]
    public async Task TestRetrieve()
    {
        // Arrange
        var sut = new Predictor.RetrieveSalesSqlServer.Implementations.RetrieveSales(_configuration["ConnectionStringSqlExpressOne"]!);

        // Act
        var result = await sut.Retrieve(_dateToRetrieve, "Utica");

        // Assert
        Assert.True(result > 0);
    }
}