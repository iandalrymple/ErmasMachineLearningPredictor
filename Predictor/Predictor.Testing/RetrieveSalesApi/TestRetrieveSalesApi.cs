using Microsoft.Extensions.Configuration;
using Predictor.Testing.Supporting;

namespace Predictor.Testing.RetrieveSalesApi;

public class TestRetrieveSalesApi
{
    private readonly IConfiguration _configuration = ConfigurationSingleton.Instance;
    private readonly DateTime _dateToRetrieve = DateTime.SpecifyKind(new DateTime(year: 2024, month: 5, day: 5), DateTimeKind.Utc);

    [Fact]
    public async Task TestAggregateModelIntoSales()
    {
        // Arrange
        var rawWeatherString = Properties.Resources.CheckListModelExample;

        // Act


        // Assert
        Assert.True(true);
    }

    [Fact]
    public async Task TestRetrieve()
    {
        // Arrange
        var guidDictionary = new Dictionary<string, string>
        {
            {"UTICA", _configuration["UticaFocusGuid"]!},
            {"WARREN", _configuration["WarrenFocusGuid"]!}
        };
        var sut = new Predictor.RetrieveSalesApi.Implementations.RetrieveSales(_configuration["PublicShiftFour"]!, _configuration["PrivateShiftFour"]!, guidDictionary);

        // Act
        var result = await sut.Retrieve(_dateToRetrieve, "Utica");

        // Assert
        Assert.True(result > 0);
    }
}