using Microsoft.Extensions.Logging;
using Predictor.Domain.Abstractions;

namespace Predictor.Domain.Implementations;

public class MainTask : IMainTask
{
    private readonly ILogger<MainTask> _logger;
    private readonly IRetrieveWeather _retriever;

    public MainTask(ILogger<MainTask> logger, IRetrieveWeather retriever)
    {
        _logger = logger;
        _retriever = retriever;
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