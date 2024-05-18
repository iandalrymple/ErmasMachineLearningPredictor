using Microsoft.Extensions.Logging;
using Predictor.Domain.Abstractions;

namespace Predictor.Console;

internal class MainApp : IMainApp
{
    private readonly ILogger<MainApp> _logger;

    public MainApp(ILogger<MainApp> logger)
    {
        _logger = logger;
    }

    public async Task Run()
    {
        while (true)
        {
            await Task.Delay(5000);
            _logger.LogInformation(DateTime.Now.ToString());
        }
    }
}