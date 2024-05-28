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

        // Assert
        Assert.NotNull(result);
    }
}