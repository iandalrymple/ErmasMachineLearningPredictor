using Microsoft.Extensions.Configuration;
using BasicEmailLibrary.Lib;

namespace Predictor.Console.Composition
{
    internal class BasicEmailComposition
    {
        static BasicEmail CreateBasicEmailObject(IConfiguration config)
        {
            // Build up the params 
            var emailParams = new BasicEmailCtorParamModel
            {
                OwnedEmailAddress = config["OwnedEmailAddress"],
                OwnedPassword = config["OwnedPassword"],
                SmtpHostName = config["SmtpHostName"],
                SmtpPortNumber = Convert.ToInt32(config["SmtpPortNumber"]),
                ImapHostName = config["ImapHostName"],
                ImapPortNumber = Convert.ToInt32(config["ImapPortNumber"])
            };

            // Create the library object 
            return new BasicEmail(emailParams);
        }
    }
}
