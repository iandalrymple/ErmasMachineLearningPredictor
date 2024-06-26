﻿using Predictor.Domain.Implementations.States;
using Predictor.Domain.Models.StateModels;
using Predictor.Domain.Models;
using Predictor.Domain.System;
using Microsoft.Extensions.Configuration;
using Predictor.RetrieveSalesApi.Implementations;
using Predictor.Testing.Supporting;

namespace Predictor.Testing.Domain;

public class TestStateRetrieveCurrentSales
{
    private readonly IConfiguration _config = ConfigurationSingleton.Instance;

    [Theory]
    [InlineData(2024, 5, 15)]
    [InlineData(2024, 5, 22)]
    public async Task TestExecute_Happy(int year, int month, int day)
    {
        // Arrange
        var guidDictionary = new Dictionary<string, string>
        {
            {"UTICA", _config["UticaFocusGuid"]!},
            {"WARREN", _config["WarrenFocusGuid"]!}
        };
        var dateToCheck = new DateTime(year: year, month: month, day: day);
        var retriever = new RetrieveSales(_config["PublicShiftFour"]!, _config["PrivateShiftFour"]!, guidDictionary);
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
        Assert.NotNull(container.StateResults.StateCurrentSalesResults);
    }
}