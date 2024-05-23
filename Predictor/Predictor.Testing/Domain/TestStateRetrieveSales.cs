using Predictor.Domain.Implementations.States;
using Predictor.Domain.Models.StateModels;
using Predictor.Domain.Models;
using Predictor.Domain.System;
using Predictor.RetrieveSales.Implementations;
using Microsoft.Extensions.Configuration;

namespace Predictor.Testing.Domain;

public class TestStateRetrieveSales
{
    private readonly IConfiguration _config = new ConfigurationBuilder()
        .AddJsonFile("testSettings.json")
        .Build();

    [Theory]
    [InlineData(2024, 5, 15)]
    [InlineData(2024, 5, 22)]
    public async Task TestExecute_Happy(int year, int month, int day)
    {
        // Arrange
        var dateToCheck = new DateTime(year: year, month: month, day: day);
        var retriever = new RetrieveSalesMock();
        var sut = new StateRetrieveCurrentSales(retriever);
        var container = new FsmStatefulContainer
        {
            CurrentState = PredictorFsmStates.CurrentSalesRetrieve,
            StoreLocation = _config.GetSection("StoreLocation")
                .Get<List<StoreLocation>>()!
                .First(storeLocation => storeLocation.Name.Equals("Utica", StringComparison.OrdinalIgnoreCase)),
            StateResults = new StatesCombinedResultModel(),
            DateToCheck = dateToCheck
        };

        // Act
        await sut.Execute(container);

        // Assert
        Assert.Equal(PredictorFsmStates.CurrentSalesRetrieve + 1, container.CurrentState);
        Assert.NotNull(container.StateResults.StateSalesResults);
    }
}