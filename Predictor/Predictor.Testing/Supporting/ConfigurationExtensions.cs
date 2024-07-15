using Microsoft.Extensions.Configuration;
using Predictor.Domain.Models;

namespace Predictor.Testing.Supporting;

internal static class ConfigurationExtensions
{
    internal static (double longitude, double latitude) Coordinates(this IConfiguration config, string storeName)
    {
        var location = config.GetSection("StoreLocation").Get<List<StoreLocation>>()!.First(storeLocation =>
            storeLocation.Name.Equals(storeName, StringComparison.OrdinalIgnoreCase));

        return (location.Longitude, location.Latitude);
    }
}