using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Predictor.RetrieveSalesApi.Models;
using Predictor.Testing.Supporting;
using System;
using BasicEmailLibrary.Lib;
using Predictor.Console.Composition;
using Predictor.Domain.Extensions;

namespace Predictor.Testing.RetrieveSalesEmail
{
    public class TestRetrieveSalesEmail
    {
        private readonly IConfiguration _configuration = ConfigurationSingleton.Instance;
        private readonly DateTime _dateToRetrieve = DateTime.SpecifyKind(new DateTime(year: 2024, month: 5, day: 5), DateTimeKind.Utc);

        [Fact]
        public void TestAggregateModelIntoSales()
        {
            // Arrange
            var basicEmail = BasicEmailComposition.CreateBasicEmailObject(_configuration);

            var rawWeatherString = Properties.Resources.CheckListModelExample;
            var modelled = JsonConvert.DeserializeObject<List<Root>>(rawWeatherString);

            // Act
            var salesTotal = modelled!.Sum(check => check.total);
            var voidTotal = modelled!.Sum(check => check.void_total);
            var firstOrderMinutes = modelled!
                .Where(check => check.time_opened.Year > 2020)
                .Min(check => check.time_opened.MinutesIntoDayForCertainDateTime());
            var lastOrderMinutes = modelled!
                .Where(check => check.time_opened.Year > 2020)
                .Max(check => check.time_opened.MinutesIntoDayForCertainDateTime());

            // Assert
            Assert.True(salesTotal - voidTotal > 0);
            Assert.True(firstOrderMinutes > 0);
            Assert.True(lastOrderMinutes > 0);
            Assert.True(lastOrderMinutes > firstOrderMinutes);
        }
    }
}
