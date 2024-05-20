namespace Predictor.Domain.Models;

public class WeatherRetrieveParamModel
{
    public double Longitude { get; init; }
    public double Latitude { get; init; }
    public DateTime DateTime { get; init; }
}