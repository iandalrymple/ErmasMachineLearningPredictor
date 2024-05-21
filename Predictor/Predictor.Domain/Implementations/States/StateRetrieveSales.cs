using Predictor.Domain.Abstractions;
using Predictor.Domain.Models;
using Predictor.Domain.Models.StateModels;
using Predictor.Domain.System;

namespace Predictor.Domain.Implementations.States;

public class StateRetrieveSales : IFsmState
{
    private readonly IRetrieveSales _retrieveSales;

    public StateRetrieveSales(IRetrieveSales retrieveSales)
    {
        State = PredictorFsmStates.SalesRetrieve;
        _retrieveSales = retrieveSales;
    }

    public PredictorFsmStates State { get; init; }

    public Task Execute(FsmStatefulContainer container)
    {
        var sales = _retrieveSales.Retrieve(DateTime.Now).Result;
        container.StateResults.StateSalesResults = new StateSalesResultModel
        {
            SalesAtThree = sales
        };
        container.CurrentState++;
        return Task.CompletedTask;
    }
}