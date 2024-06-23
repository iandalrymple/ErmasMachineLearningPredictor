using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Predictor.RetrieveSalesApi.Models;
using Predictor.Testing.Supporting;
using System;
using BasicEmailLibrary.Lib;
using Microsoft.VisualBasic;
using Predictor.Console.Composition;
using Predictor.Domain.Extensions;

namespace Predictor.Testing.RetrieveSalesEmail
{
    public class TestRetrieveSalesEmail
    {
        private readonly IConfiguration _configuration = ConfigurationSingleton.Instance;
        private readonly DateTime _dateToRetrieve = DateTime.SpecifyKind(new DateTime(year: 2024, month: 5, day: 5), DateTimeKind.Utc);

        [Theory]
        [InlineData(2024, 6, 19, 2.3)]
        [InlineData(2024, 6, 20, 5.3)]
        public async Task TestAggregateModelIntoSales(int year, int month, int day, decimal salesAtThree)
        {
            // Arrange
            var basicEmail = BasicEmailComposition.CreateBasicEmailObject(_configuration);
            var sut = new Predictor.RetrieveSalesEmail.Implementations.RetrieveSales(basicEmail);

            // Act
            var dateTime = new DateTime(year: year, month: month, day: day);
            var result = await sut.Retrieve(dateTime, "Utica");

            // Assert
            Assert.Equal(salesAtThree, result.SalesAtThree);
            Assert.True(result.FirstOrderMinutesInDay > 0);
            Assert.True(result.LastOrderMinutesInDay > 0);
            Assert.True(result.FirstOrderMinutesInDay < result.LastOrderMinutesInDay);
        }

        [Fact]
        public void TestCsvModelFirstOrder()
        {
            // Arrange
            var rawCsvString = Properties.Resources.EmailThreePm;
            var csvModel = new Predictor.RetrieveSalesEmail.Models.CsvModel(rawCsvString);

            // Act
            //var result = Predictor.RetrieveSalesEmail.Implementations.RetrieveSales.FirstOrderOfDay(csv!);

            // Assert
        }
    }
}
