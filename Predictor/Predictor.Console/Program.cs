using BasicEmailLibrary.Lib;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Predictor.Console.Composition;
using Predictor.Domain.Abstractions;
using Predictor.Domain.Implementations;
using Predictor.Domain.Implementations.States;
using Predictor.Domain.Models;
using Predictor.Domain.Models.StateModels;
using Predictor.Domain.System;
using Predictor.InsertSalesSqlite.Implementations;
using Predictor.RetrieveOwmWeather.Implementations;
using Serilog;
using System.Collections.Concurrent;


// NOTE - DI model fashioned from here
// https://www.youtube.com/watch?v=GAOCe-2nXqc

// Constants 
const bool useMockWeather = true;

// Args passed in. 
const int storeArgIndex = 0;
const int dateArgIndex = 1;
var storeArg = args[storeArgIndex];
var dateArg = Convert.ToDateTime(args[dateArgIndex]);

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
            // Create a transient for the basic email library. 
            services.AddTransient<BasicEmail>(x => BasicEmailComposition.CreateBasicEmailObject(config));

            // Set up the weather retriever. Need to be able to bypass for integration
            // testing, so we do not use up API hits on OWM API usage.
            if (useMockWeather)
            {
                Log.Logger.Error("Using the mock weather retriever.");
                services.AddSingleton<IRetrieveWeather>(x => new RetrieveWeatherMock());
            }
            else
            {
                // Order matters here with the decorate pattern. This is using Scrutor.
                services.AddSingleton<IRetrieveWeather>(x =>
                    new RetrieveWeather(config["BaseWeatherUri"]!, config["AppId"]!));
                services.Decorate<IRetrieveWeather, LoggingDecoratorRetrieveWeather>();
            }

            services.AddSingleton(x =>
            {
                // Declare at top since we want to add as we go.
                var stateDictionary = new ConcurrentDictionary<PredictorFsmStates, IFsmState>();

                // Weather 
                var weatherRetriever = x.GetRequiredService<IRetrieveWeather>();
                var stateWeather = new StateWeather(weatherRetriever);
                stateDictionary.TryAdd(stateWeather.State, stateWeather);

                // CurrentSalesRetrieve
                var cacheRetriever = new Predictor.RetrieveSalesSqlite.Implementations.RetrieveSales(config["ConnectionStringSqlite"]!);
                var cacheInserter = new InsertSales(config["ConnectionStringSqlite"]!);
                var emailRetriever = new Predictor.RetrieveSalesEmail.Implementations.RetrieveSales(
                    x.GetRequiredService<BasicEmail>(), 
                    cacheRetriever, 
                    cacheInserter, 
                    x.GetRequiredService<ILogger<Predictor.RetrieveSalesEmail.Implementations.RetrieveSales>>());
                var stateCurrentSalesRetriever = new StateRetrieveCurrentSales(emailRetriever);
                stateDictionary.TryAdd(stateCurrentSalesRetriever.State, stateCurrentSalesRetriever);

                // Left off here 

                // Bounce back the collection of states.
                return stateDictionary;
            });

            services.AddSingleton(x =>
            {
                return new FsmStatefulContainer
                {
                    CurrentState = PredictorFsmStates.Weather,
                    StoreLocation = config.GetSection("StoreLocation").Get<List<StoreLocation>>()!.First(storeLocation => storeLocation.Name.Equals(storeArg, StringComparison.OrdinalIgnoreCase)),
                    StateResults = new StatesCombinedResultModel(),
                    DateToCheck = dateArg
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
