using BasicEmailLibrary.Lib;
using Predictor.Domain.Abstractions;
using Predictor.Domain.Models.StateModels;

namespace Predictor.RetrieveSalesEmail.Implementations
{
    internal class RetrieveSales : IRetrieveSales<StateCurrentSalesResultModel>
    {
        private readonly BasicEmail _email;

        public RetrieveSales(BasicEmail email)
        {
            _email = email;
        }

        public Task<StateCurrentSalesResultModel> Retrieve(DateTime dateTime, string storeName)
        {
            // Get all the emails 
            var emails = _email.GetAllUnreadEmail();

            // Now we need to fish through them and grab the one from 3 pm. 
        }
    }
}
