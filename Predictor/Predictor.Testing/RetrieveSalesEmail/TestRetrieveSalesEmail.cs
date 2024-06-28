using Microsoft.Extensions.Configuration;
using Predictor.Console.Composition;
using Predictor.Testing.Supporting;

namespace Predictor.Testing.RetrieveSalesEmail
{
    public class TestRetrieveSalesEmail
    {
        private readonly IConfiguration _configuration = ConfigurationSingleton.Instance;

        [Theory]
        [InlineData(2024, 6, 19, 858.53, 660, 0)]
        [InlineData(2024, 6, 20, 5.3, 250, 0)]
        public async Task TestRetrieve(int year, int month, int day, decimal salesAtThree, uint firstOrderTime, uint lastOrderTime)
        {
            // Arrange
            var basicEmail = BasicEmailComposition.CreateBasicEmailObject(_configuration);
            var sut = new Predictor.RetrieveSalesEmail.Implementations.RetrieveSales(basicEmail);

            // Act
            var dateTime = new DateTime(year: year, month: month, day: day);
            var result = await sut.Retrieve(dateTime, "Utica");

            // Assert
            Assert.Equal(salesAtThree, result.SalesAtThree, 0);
            Assert.Equal(firstOrderTime, result.FirstOrderMinutesInDay);
            Assert.Equal(lastOrderTime, result.LastOrderMinutesInDay);
        }

        [Fact]
        public void TestCsvModel()
        {
            // Arrange
            var rawCsvString = Properties.Resources.EmailThreePm;

            // Act
            var csvModel = new Predictor.RetrieveSalesEmail.Models.CsvModel(rawCsvString);

            // Assert
            Assert.Equal((uint)660, csvModel.FirstOrderInMinutesFromStartOfDay);
            Assert.Equal(858.53m, csvModel.SalesAtThree, 0);
        }
    }
}
