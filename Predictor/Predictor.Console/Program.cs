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
using Predictor.PredictingEnginePython.Implementations;
using Predictor.RetrieveHolidays.Implementations;
using Predictor.RetrieveOwmWeather.Implementations;
using Serilog;
using System.Collections.Concurrent;


// NOTE - DI model fashioned from here
// https://www.youtube.com/watch?v=GAOCe-2nXqc

// Constants 
const bool useMockWeather = false;

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
            // State dictionary
            services.AddSingleton(x =>
            {
                // Declare at top since we want to add as we go.
                var stateDictionary = new ConcurrentDictionary<PredictorFsmStates, IFsmState>();

                // Weather 
                // Set up the weather retriever. Need to be able to bypass for integration
                // testing, so we do not use up API hits on OWM API usage.
                IRetrieveWeather weatherRetriever;
                if (useMockWeather)
                {
                    Log.Logger.Error("Using the mock weather retriever.");
                    weatherRetriever = new RetrieveWeatherMock();
                }
                else
                {
                    var weatherCacheRetriever = new Predictor.RetrieveOwmWeatherSqlite.Implementations.RetrieveWeather(config["ConnectionStringSqliteWeatherCache"]!);
                    weatherRetriever = new RetrieveWeather(
                        config["BaseWeatherUri"]!, 
                        config["AppId"]!, 
                        x.GetRequiredService<ILogger<RetrieveWeather>>(),
                        weatherCacheRetriever);
                }
                var stateWeather = new StateWeather(weatherRetriever);
                stateDictionary.TryAdd(stateWeather.State, stateWeather);

                // CurrentSalesRetrieve
                var cacheRetriever = new Predictor.RetrieveSalesSqlite.Implementations.RetrieveSales(config["ConnectionStringSqliteSalesCache"]!);
                var cacheInserter = new InsertSales(config["ConnectionStringSqliteSalesCache"]!);
                var emailRetriever = new Predictor.RetrieveSalesEmail.Implementations.RetrieveSales(
                    BasicEmailComposition.CreateBasicEmailObject(config), 
                    cacheRetriever, 
                    cacheInserter, 
                    x.GetRequiredService<ILogger<Predictor.RetrieveSalesEmail.Implementations.RetrieveSales>>());
                var stateCurrentSalesRetriever = new StateRetrieveCurrentSales(emailRetriever);
                stateDictionary.TryAdd(stateCurrentSalesRetriever.State, stateCurrentSalesRetriever);

                // HistoricSalesRetrieve
                var historicRetriever = new Predictor.RetrieveSalesSqlServer.Implementations.RetrieveSales(config["ConnectionStringSqlExpressOne"]!);
                var stateHistoricSalesRetrieve = new StateRetrieveHistoricSales(historicRetriever);
                stateDictionary.TryAdd(stateHistoricSalesRetrieve.State, stateHistoricSalesRetrieve);

                // Aggregate
                var retrieveHolidays = new RetrieveHolidays();
                var stateAggregate = new StateAggregate(retrieveHolidays);
                stateDictionary.TryAdd(stateAggregate.State, stateAggregate);

                // Predict
                var pythonEngine = new PredictingEnginePythonImpl(
                    config["PythonExe"]!,
                    config["PythonWorkingDirectory"]!,
                    config.GetSection("PythonArgs").Get<string[]>()!,
                    x.GetRequiredService<ILogger<PredictingEnginePythonImpl>>());
                var statePredict = new StatePredict(pythonEngine);
                stateDictionary.TryAdd(statePredict.State, statePredict);

                // Bounce back the collection of states.
                return stateDictionary;
            });

            // Container 
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

            // Conductor
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
