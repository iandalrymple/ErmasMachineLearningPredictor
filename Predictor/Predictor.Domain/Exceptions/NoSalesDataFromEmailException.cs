namespace Predictor.Domain.Exceptions;

public class NoSalesDataFromEmailException : Exception
{
    public NoSalesDataFromEmailException()
    {
        
    }

    public NoSalesDataFromEmailException(DateTime dateTime, string storeName, string otherContent)
        : base($"Invalid sales data returned from email server for DateTime: {dateTime} at StoreName: {storeName} with message: {otherContent}")
    {

    }
}