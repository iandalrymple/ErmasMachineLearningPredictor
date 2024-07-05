using Microsoft.Extensions.Configuration;
using Predictor.Domain.Models;
using Predictor.Testing.RetrieveSalesSqlite;
using Predictor.Testing.Supporting;

namespace Predictor.Testing.InsertSalesSqlite
{
    public class TestInsertSalesSqlite
    {
        private readonly IConfiguration _configuration = ConfigurationSingleton.Instance;

        [Fact]
        public async Task TestInsert()
        {
            string? tempDatabaseName = null;
            try
            {
                // Arrange
                var setUpResult = await SqliteHelpers.SetUpDataBaseWithRecords("Utica", new DateTime(2024, 1, 1), _configuration, 3);
                tempDatabaseName = setUpResult!.dbFileName;
                var sut = new Predictor.InsertSalesSqlite.Implementations.InsertSales(setUpResult.connString!);
                var insertionDataOne = new CacheModel
                {
                    SalesThreePm = 22.3m,
                    FirstOrderMinutesIntoDay = 668,
                    Store = "Utica",
                    Date = "2024-05-01",
                };
                var insertionDataTwo = new CacheModel
                {
                    SalesThreePm = 22.3m,
                    FirstOrderMinutesIntoDay = 668,
                    Store = "Warren",
                    Date = "2024-05-01",
                };

                // Act
                var resultOne = await sut.Insert(insertionDataOne);
                var resultTwo = await sut.Insert(insertionDataTwo);

                // Assert
                Assert.True(resultOne);
                Assert.True(resultTwo);
            }
            finally
            {
                if (!string.IsNullOrEmpty(tempDatabaseName) && File.Exists(tempDatabaseName))
                {
                    File.Delete(tempDatabaseName);
                }
            }
        }
    }
}
