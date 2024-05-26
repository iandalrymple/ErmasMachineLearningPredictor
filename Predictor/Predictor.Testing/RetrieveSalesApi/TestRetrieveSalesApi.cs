using Microsoft.Extensions.Configuration;
using Predictor.Testing.Supporting;

namespace Predictor.Testing.RetrieveSalesApi;

public class TestRetrieveSalesApi
{
    private readonly IConfiguration _configuration = ConfigurationSingleton.Instance;
    private readonly DateTime _dateToRetrieve = DateTime.SpecifyKind(new DateTime(year: 2023, month: 6, day: 19), DateTimeKind.Utc);

    [Fact]
    public async Task TestRetrieve()
    {
        // Arrange
        var sut = new Predictor.RetrieveSalesApi.Implementations.RetrieveSales(_configuration["KeyOne"]!, _configuration["KeyTwo"]!);

        // Act
        var result = await sut.Retrieve(_dateToRetrieve, "Utica");

        // Assert
        Assert.True(result > 0);
    }
}