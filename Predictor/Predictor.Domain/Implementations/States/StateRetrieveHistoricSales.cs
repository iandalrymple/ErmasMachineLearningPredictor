using Predictor.Domain.Abstractions;
using Predictor.Domain.Models;
using Predictor.Domain.Models.StateModels;
using Predictor.Domain.System;

namespace Predictor.Domain.Implementations.States
{
    public class StateRetrieveHistoricSales : IFsmState
    {
        private readonly IRetrieveSales<decimal> _retriever;

        public StateRetrieveHistoricSales(IRetrieveSales<decimal> retriever)
        {
            State = PredictorFsmStates.HistoricSalesRetrieve;
            _retriever = retriever;
        }

        public PredictorFsmStates State { get; init; }

        public async Task Execute(FsmStatefulContainer container)
        {
            // Get the day before. 
            var dayBeforeSales = await _retriever.Retrieve(container.DateToCheck.AddDays(-1), container.StoreLocation.Name);

            // Get two days before.
            var twoDaysBeforeSales = await _retriever.Retrieve(container.DateToCheck.AddDays(-2), container.StoreLocation.Name);

            // Stuff into the container. 
            container.StateResults.StateHistoricSalesResults = new StateHistoricSalesResultModel
            {
                SalesDayBefore = dayBeforeSales,
                SalesTwoDaysBefore = twoDaysBeforeSales
            };

            // Increment the state.
            container.CurrentState++;
        }
    }
}
