using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Predictor.Domain.Models;
using Predictor.PredictingEnginePython.Implementations;
using Predictor.RetrieveOwmWeather.Implementations;
using Predictor.Testing.Supporting;

namespace Predictor.Testing.PredictingEnginePython;

public class TestPredictingEnginePython
{
    private readonly IConfiguration _config;
    private readonly ILogger<PredictingEnginePythonImpl> _logger;

    public TestPredictingEnginePython()
    {
        _config = ConfigurationSingleton.Instance;

        var serviceProvider = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();
        var factory = serviceProvider.GetService<ILoggerFactory>();
        _logger = factory!.CreateLogger<PredictingEnginePythonImpl>();
    }

    [Fact]
    public async Task TestRunProcessAsync()
    {
        // Arrange
        var sut = new PredictingEnginePythonImpl(
            _config["PythonExe"]!, 
            _config["PythonWorkingDirectory"]!, 
            _config.GetSection("PythonArgs").Get<string[]>()!, 
            _logger);
        var rawFeatureString = Properties.Resources.PredictorFeatures;
        var inputParams = new PredictingEngineParameterModel{ StoreName = "Utica", Features = rawFeatureString};

        // Act
        var result = await sut.PredictAsync(inputParams);

        // Assert
        Assert.NotNull(result);
    }
}