using Microsoft.Extensions.Configuration;

namespace Predictor.Testing.Supporting;

internal class ConfigurationSingleton
{
    private readonly IConfiguration _config;

    private static ConfigurationSingleton? _instance;

    private ConfigurationSingleton()
    {
        _config = new ConfigurationBuilder()
            .AddJsonFile("testSettings.json")
            .Build();
    }

    public static IConfiguration Instance
    {
        get
        {
            _instance ??= new ConfigurationSingleton();
            return _instance._config;
        }
    }
}