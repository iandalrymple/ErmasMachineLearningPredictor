namespace Predictor.Domain.Models;

public class StoreLocation
{
    public required string StoreName { get; init; }
    public required double Latitude { get; init; }
    public required double Longitude { get; init;}
}