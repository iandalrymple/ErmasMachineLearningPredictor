using Predictor.Domain.Abstractions;
using Predictor.Domain.Models.StateModels;

namespace Predictor.RetrieveSalesEmail.Implementations
{
    internal class RetrieveSales : IRetrieveSales<StateCurrentSalesResultModel>
    {
        public Task<StateCurrentSalesResultModel> Retrieve(DateTime dateTime, string storeName)
        {
            throw new NotImplementedException();
        }
    }
}
