using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

using Predictor.Console.Composition;

using Predictor.Domain.Abstractions;
using Predictor.Domain.Implementations;
using Predictor.Domain.System;
using Predictor.Domain.Implementations.States;
using Predictor.Domain.Models;
using Predictor.RetrieveOwmWeather.Implementations;


// NOTE - DI model fashioned from here
// https://www.youtube.com/watch?v=GAOCe-2nXqc

try
{
    // Config
    var builder = new ConfigurationBuilder();
    builder.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("consoleSettings.json", optional: false, reloadOnChange: true);
    var config = builder.Build();

    // Logger folders
    var appDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "ErmasMachineLearningPredictor");
    if (Directory.Exists(appDirectory) == false)
    {
        Directory.CreateDirectory(appDirectory);
    }
    appDirectory = Path.Combine(appDirectory, "Logs");
    if (Directory.Exists(appDirectory) == false)
    {
        Directory.CreateDirectory(appDirectory);
    }
    var logFileName = Path.Combine(appDirectory, "ServiceLogs.log");

    // Logging set up.
    var logLevel = config.GetSection("Serilog").GetSection("MinimumLevel")["Default"];
    var levelSwitch = LoggerComposition.TranslateLogLevel(logLevel);
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(config)
        .Enrich.FromLogContext()
        .MinimumLevel.ControlledBy(levelSwitch)
        .WriteTo.File(logFileName, rollOnFileSizeLimit: true, fileSizeLimitBytes: 1048576)
#if DEBUG
        .WriteTo.Console()
#endif
        .WriteTo.Seq("http://localhost:5341",
            apiKey: config["SeqApiKey"],
            controlLevelSwitch: levelSwitch)
        .CreateLogger();
    Log.Logger.Information("{AppName} is starting.", System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);

    // Add services.
    var host = Host.CreateDefaultBuilder()
        .ConfigureServices((context, services) =>
        {
            // Order matters here with the decorate pattern. This is using Scrutor.
            services.AddSingleton<IRetrieveWeather>(x => new RetrieveWeather(config["BaseWeatherUri"]!));
            services.Decorate<IRetrieveWeather, LoggingDecoratorRetrieveWeather>();

            services.AddSingleton(x =>
            {
                var retriever = x.GetRequiredService<IRetrieveWeather>();
                var stateWeather = new StateWeather(retriever);
                var stateDictionary = new ConcurrentDictionary<PredictorFsmStates, IFsmState>();
                stateDictionary.TryAdd(stateWeather.State, stateWeather);
                return stateDictionary;
            });

            services.AddSingleton(x =>
            {
                return new FsmStatefulContainer
                {
                    CurrentState = PredictorFsmStates.Weather,
                    StoreLocation = config.GetSection("StoreLocation").Get<List<StoreLocation>>()!.First(x => x.Name.Equals(args[0], StringComparison.OrdinalIgnoreCase))
                };
            });
            services.AddSingleton<IFsmConductor, FsmConductor>();
        })
        .UseSerilog()
        .Build();

    // Start the main task.
    var svc = ActivatorUtilities.CreateInstance<FsmConductor>(host.Services);
    await svc.Execute();
}
catch (Exception ex)
{
    Log.Logger.Error("{Exception}", ex);
}
finally
{
    await Log.CloseAndFlushAsync();
}
