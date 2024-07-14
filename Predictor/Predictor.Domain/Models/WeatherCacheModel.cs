namespace Predictor.Domain.Models;

public class WeatherCacheModel
{
    public int DbId { get; init; }
    public double Longitude { get; init; }
    public double Latitude { get; init; }
    public string DateTime { get; init; } = string.Empty;   
    public string WeatherJson { get; init; } = string.Empty;
}