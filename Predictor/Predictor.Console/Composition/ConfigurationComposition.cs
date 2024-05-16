using Microsoft.Extensions.Configuration;

namespace Predictor.Console.Composition;

internal class ConfigurationComposition
{
    internal static IConfiguration BuildConfiguration(string fileName)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile(fileName)
            .Build();
        return config;
    }
}