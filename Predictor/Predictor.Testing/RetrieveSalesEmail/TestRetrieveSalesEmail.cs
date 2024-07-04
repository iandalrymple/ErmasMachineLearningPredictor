using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Predictor.Console.Composition;
using Predictor.Testing.Mocks;
using Predictor.Testing.RetrieveSalesSqlite;
using Predictor.Testing.Supporting;

namespace Predictor.Testing.RetrieveSalesEmail
{
    public class TestRetrieveSalesEmail
    {
        private readonly IConfiguration _configuration = ConfigurationSingleton.Instance;
        private readonly ILogger<Predictor.RetrieveSalesEmail.Implementations.RetrieveSales> _logger;

        public TestRetrieveSalesEmail()
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            _logger = factory!.CreateLogger<Predictor.RetrieveSalesEmail.Implementations.RetrieveSales>();
        }

        [Theory]
        [InlineData(2024, 6, 19, 1127.85, 660, 0)]
        [InlineData(2024, 6, 20, 858.53, 660, 0)]
        public async Task TestRetrieve_NoCacheHit(int year, int month, int day, decimal salesAtThree, uint firstOrderTime, uint lastOrderTime)
        {
            // Arrange
            var basicEmail = BasicEmailComposition.CreateBasicEmailObject(_configuration);
            var mockCacheRetriever = new RetrieveSalesAlwaysNullMock();
            var sut = new Predictor.RetrieveSalesEmail.Implementations.RetrieveSales(basicEmail, mockCacheRetriever, _logger);

            // Act
            var dateTime = new DateTime(year: year, month: month, day: day);
            var result = await sut.Retrieve(dateTime, "Utica");

            // Assert
            Assert.Equal(salesAtThree, result.SalesAtThree, 0);
            Assert.Equal(firstOrderTime, result.FirstOrderMinutesInDay);
            Assert.Equal(lastOrderTime, result.LastOrderMinutesInDay);
        }

        [Theory]
        [InlineData("Utica",2024, 6, 19, 1000.00, 650, uint.MaxValue)]
        [InlineData("Utica",2024, 6, 20, 1000.00, 650, uint.MaxValue)]
        public async Task TestRetrieve_WithCacheHit(string store, int year, int month, int day, decimal salesAtThree, uint firstOrderTime, uint lastOrderTime)
        {
            // Arrange
            var dateTime = new DateTime(year: year, month: month, day: day);
            var (connString, _) = await TestRetrieveSalesSqlite.SetUpDataBase(store, dateTime, _configuration, 3);
            var basicEmail = BasicEmailComposition.CreateBasicEmailObject(_configuration);
            var cacheRetriever = new Predictor.RetrieveSalesSqlite.Implementations.RetrieveSales(connString!);
            var sut = new Predictor.RetrieveSalesEmail.Implementations.RetrieveSales(basicEmail, cacheRetriever, _logger);

            // Act
            var result = await sut.Retrieve(dateTime, store);

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
