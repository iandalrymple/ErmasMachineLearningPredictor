namespace Predictor.Domain.Models;

public class WeatherRetrieveParamModel
{
    public double Longitude { get; init; }
    public double Latitude { get; init; }

    public string AppId { get; init; } = string.Empty;

    public DateTime DateTime { get; init; }
}