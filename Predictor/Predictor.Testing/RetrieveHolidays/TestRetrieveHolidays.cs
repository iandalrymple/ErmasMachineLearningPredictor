// using Newtonsoft.Json;

namespace Predictor.Testing.RetrieveHolidays;

public class TestRetrieveHolidays
{
    [Fact]
    public async Task TestAggregateModelIntoSales()
    {
        // Arrange
        var sut = new Predictor.RetrieveHolidays.Implementations.RetrieveHolidays();

        // Act
        var result = await sut.GetHolidays(2024);
        
        // Uncomment to print results if needed for test setup (maybe for another year). 
        // var stringify = JsonConvert.SerializeObject(result);
        // await File.WriteAllTextAsync("Holidays2024.txt" ,stringify, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
    }
}