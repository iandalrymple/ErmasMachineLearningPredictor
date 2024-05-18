using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Predictor.Console;
using Predictor.Domain.Abstractions;
using Predictor.RetrieveOwmWeather.Implementations;
using Serilog;
using Serilog.Core;
using Serilog.Events;

// Config
var builder = new ConfigurationBuilder();
BuildConfig(builder);
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
var levelSwitch = TranslateLogLevel(logLevel);
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

Log.Logger.Information("Application Starting");

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddTransient<IRetrieveWeather, LoggingDecoratorRetrieveWeather>();
        services.AddSingleton<MainApp, MainApp>();
    })
    .UseSerilog()
    .Build();

var svc = ActivatorUtilities.CreateInstance<MainApp>(host.Services);
await svc.Run();

static void BuildConfig(IConfigurationBuilder builder)
{
    builder.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("consoleSettings.json", optional: false, reloadOnChange: true);
}

static LoggingLevelSwitch TranslateLogLevel(string? logLevel)
{
    LoggingLevelSwitch levelSwitch;
    if (Enum.TryParse(logLevel, out LogEventLevel parsedLevel))
    {
        levelSwitch = new LoggingLevelSwitch
        {
            MinimumLevel = parsedLevel
        };
    }
    else
    {
        if (string.Equals(logLevel, "Trace", StringComparison.OrdinalIgnoreCase))
        {

            levelSwitch = new LoggingLevelSwitch
            {
                MinimumLevel = LogEventLevel.Verbose
            };
        }
        else
        {
            levelSwitch = new LoggingLevelSwitch
            {
                MinimumLevel = LogEventLevel.Information
            };
        }
    }

    return levelSwitch;
}







//// First build the configuration object.
//var builder = new ConfigurationBuilder();
//var config = builder.AddJsonFile("consoleSettings.json").Build();

//// Sign up for the unhandled exception which forces the flush.
//AppDomain.CurrentDomain.UnhandledException += AppUnhandledException;

//// Specifying the configuration for serilog - using forces a flush.
//var abstractSerilogger = LoggerComposition.BuildLogger(config, "ErmasMachineLearningPredictor");
//using var logger = abstractSerilogger as Serilog.Core.Logger;
//logger!.Information("{AppName} is starting.", System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);

//try
//{



//    // Create the retriever.
//    var rco = new RestClientOptions(config["BaseWeatherUri"]!);
//    var retrieveObj = new RetrieveWeather(new RestClient(rco), new RestRequest());

//    var host = Host.CreateDefaultBuilder() 
//        .ConfigureServices((context, services) =>
//        {
//            services.AddSingleton<IMainApp, MainApp>();
//            //services.AddSingleton<IRetrieveWeather>(x => new LoggingDecoratorRetrieveWeather(retrieveObj, logger))
//        })
//        .UseSerilog(dispose: true) 
//        .Build(); 
//}
//catch (Exception e)
//{
//    Console.WriteLine(e);
//}

//return;



//static void AppUnhandledException(object sender, UnhandledExceptionEventArgs e)
//{
//    if (e.ExceptionObject is not Exception exception) return;
//    Log.Logger.Error(exception, "Console application crashed");
//    if (e.IsTerminating)
//    {
//        Log.CloseAndFlush();
//    }
//}