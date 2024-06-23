using BasicEmailLibrary.Lib;
using Predictor.Domain.Abstractions;
using Predictor.Domain.Exceptions;
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
            var emails = await _email.GetAllUnreadEmail(dateTime, storeName);

            // Grab the latest file since we know they just duplicate after the one at three. 
            var sorted = emails
                .OrderByDescending(e => e.Date.LocalDateTime);
                
            // Check for null.
            if (!sorted.Any())
            {
                throw new NoSalesDataFromEmailException(dateTime, storeName, "No emails returned.");   
            }

            // Now we need to check for the time on the record. 
            //if(sorted.First().Date.LocalDateTime.Tim)

            // Now we need to fish through them and grab the one from 3 pm. 
            throw new NotFiniteNumberException();
        }
    }
}
