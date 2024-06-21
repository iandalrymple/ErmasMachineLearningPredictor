using BasicEmailLibrary.Lib;
using Predictor.Domain.Abstractions;
using Predictor.Domain.Models.StateModels;

namespace Predictor.RetrieveSalesEmail.Implementations
{
    public class RetrieveSales : IRetrieveSales<StateCurrentSalesResultModel>
    {
        private readonly BasicEmail _email;

        public RetrieveSales(BasicEmail email)
        {
            _email = email;
        }

        public async Task<StateCurrentSalesResultModel> Retrieve(DateTime dateTime, string storeName)
        {
            // Get all the emails 
            var emails = await _email.GetAllUnreadEmail();

            // Now we need to fish through them and grab the one from 3 pm. 
            throw new NotFiniteNumberException();
        }
    }
}
