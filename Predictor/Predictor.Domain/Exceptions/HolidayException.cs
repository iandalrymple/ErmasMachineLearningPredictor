namespace Predictor.Domain.Exceptions;

public class HolidayException : Exception
{
    public HolidayException()
    {
        
    }

    public HolidayException(string message)
        : base($"Error in holiday parsing: {message}.")
    {

    }
}