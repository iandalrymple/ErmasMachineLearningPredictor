using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Predictor.RetrieveSalesApi.Models;
using Predictor.Testing.Supporting;

namespace Predictor.Testing.RetrieveSalesApi;

public class TestRetrieveSalesApi
{
    private readonly IConfiguration _configuration = ConfigurationSingleton.Instance;
    private readonly DateTime _dateToRetrieve = DateTime.SpecifyKind(new DateTime(year: 2024, month: 5, day: 5), DateTimeKind.Utc);

    [Fact]
    public void TestAggregateModelIntoSales()
    {
        // Arrange
        var rawWeatherString = Properties.Resources.CheckListModelExample;
        var modelled = JsonConvert.DeserializeObject<List<Root>>(rawWeatherString);

        // Act
        var salesTotal = modelled!.Sum(check => check.total);
        var voidTotal = modelled!.Sum(check => check.void_total);

        // Assert
        Assert.True(salesTotal - voidTotal > 0);
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