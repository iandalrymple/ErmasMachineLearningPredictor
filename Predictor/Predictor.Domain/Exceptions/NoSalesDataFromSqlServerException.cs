namespace Predictor.Domain.Exceptions;

public class NoSalesDataFromSqlServerException : Exception
{
    public NoSalesDataFromSqlServerException(DateTime dateTime, string storeName, string otherContent)
        : base($"Invalid sales data returned from SQL server for DateTime: {dateTime} at StoreName: {storeName} with message: {otherContent}")
    {

    }
}