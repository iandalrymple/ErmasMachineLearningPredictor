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
            var lastEmail = emails.MaxBy(e => e.Date.LocalDateTime) ?? throw new NoSalesDataFromEmailException(dateTime, storeName, "No emails returned.");

            // Check to make sure the email has an attachment. 
            if (!lastEmail.Attachments.Any() || lastEmail.Attachments.First() is null)
            {
                throw new NoSalesDataFromEmailException(dateTime, storeName, "The selected email has no attachment.");
            }

            // Parse the attachment. 
            var rawAttachmentContents = lastEmail.Attachments.First()!;


            

            // Now we need to check for the time on the record. 
            var result = new StateCurrentSalesResultModel
            {
                FirstOrderMinutesInDay = lastEmail.
            };

            // Now we need to fish through them and grab the one from 3 pm. 
            throw new NotFiniteNumberException();
        }
    }
}
