namespace Predictor.Domain.Exceptions;

[Serializable]
public class WeatherDataNotFoundException : Exception
{
    public WeatherDataNotFoundException() { }

    public WeatherDataNotFoundException(DateTime dateTime)
        : base($"Invalid weather returned for DateTime: {dateTime}")
    {

    }
}