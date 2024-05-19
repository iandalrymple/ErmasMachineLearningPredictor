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

    // Create the dictionary of states.
    var weatherState = new StateWeather();
    var stateDictionary = new ConcurrentDictionary<PredictorFsmStates, IFsmState>();
    stateDictionary.TryAdd(weatherState.State, weatherState);

    // Stateful container.
    var stateContainer = new FsmStatefulContainer();

    // Add services.
    var host = Host.CreateDefaultBuilder()
        .ConfigureServices((context, services) =>
        {
            services.AddTransient<IRetrieveWeather>(x => new RetrieveWeather(config["BaseWeatherUri"]!));
            services.Decorate<IRetrieveWeather, LoggingDecoratorRetrieveWeather>();

            services.AddSingleton(x => stateDictionary);
            services.AddSingleton(x => stateContainer);
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
