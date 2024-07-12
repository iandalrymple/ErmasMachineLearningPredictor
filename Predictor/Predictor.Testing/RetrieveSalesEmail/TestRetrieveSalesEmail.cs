using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Predictor.Console.Composition;
using Predictor.InsertSalesSqlite.Implementations;
using Predictor.RetrieveSalesSqlite.Implementations;
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
        [InlineData("Utica",2024, 6, 19, 1127.85, 660, 0)]
        [InlineData("Utica",2024, 6, 20, 858.53, 660, 0)]
        public async Task TestRetrieve_NoCacheHit(string store, int year, int month, int day, decimal salesAtThree, uint firstOrderTime, uint lastOrderTime)
        {
            string? tempDatabaseName = null;

            try
            {
                // Arrange
                var basicEmail = BasicEmailComposition.CreateBasicEmailObject(_configuration);
                await basicEmail.MarkAllEmailsToUnread();
                var (connString, dbName) = SqliteHelpers.SetUpDataBaseNoRecordsSalesCache(_configuration);
                tempDatabaseName = dbName;
                var cacheRetriever = new RetrieveSales(connString!);
                var cacheInserter = new InsertSales(connString!);
                var sut = new Predictor.RetrieveSalesEmail.Implementations.RetrieveSales(basicEmail, cacheRetriever, cacheInserter, _logger);

                // Act
                var dateTime = new DateTime(year: year, month: month, day: day);
                var result = await sut.Retrieve(dateTime, store);

                // Assert
                Assert.Equal(salesAtThree, result.SalesAtThree, 0);
                Assert.Equal(firstOrderTime, result.FirstOrderMinutesInDay);
                Assert.Equal(lastOrderTime, result.LastOrderMinutesInDay);
            }
            finally
            {
                if (!string.IsNullOrEmpty(tempDatabaseName) && File.Exists(tempDatabaseName))
                {
                    File.Delete(tempDatabaseName);
                }
            }
        }

        [Theory]
        [InlineData("Utica",2024, 6, 19, 1000.00, 650, uint.MaxValue)]
        [InlineData("Utica",2024, 6, 20, 1000.00, 650, uint.MaxValue)]
        public async Task TestRetrieve_WithCacheHit(string store, int year, int month, int day, decimal salesAtThree, uint firstOrderTime, uint lastOrderTime)
        {
            string? tempDatabaseName = null;

            try
            {
                // Arrange
                var dateTime = new DateTime(year: year, month: month, day: day);
                var (connString, dbName) = await SqliteHelpers.SetUpDataBaseWithRecordsSalesCache(store, dateTime, _configuration, 3);
                tempDatabaseName = dbName;
                var basicEmail = BasicEmailComposition.CreateBasicEmailObject(_configuration);
                var cacheRetriever = new RetrieveSales(connString!);
                var cacheInserter = new InsertSales(connString!);
                var sut = new Predictor.RetrieveSalesEmail.Implementations.RetrieveSales(basicEmail, cacheRetriever, cacheInserter, _logger);

                // Act
                var result = await sut.Retrieve(dateTime, store);

                // Assert
                Assert.Equal(salesAtThree, result.SalesAtThree, 0);
                Assert.Equal(firstOrderTime, result.FirstOrderMinutesInDay);
                Assert.Equal(lastOrderTime, result.LastOrderMinutesInDay);
            }
            finally
            {
                if (!string.IsNullOrEmpty(tempDatabaseName) && File.Exists(tempDatabaseName))
                {
                    File.Delete(tempDatabaseName);
                }
            }
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
