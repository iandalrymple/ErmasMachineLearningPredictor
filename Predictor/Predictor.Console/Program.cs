using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Predictor.Console.Composition;
using Serilog;

// DI references below (mashed together)
// https://jkdev.me/serilog-console/
// https://dev.to/moe23/net-5-console-app-with-dependency-injection-serilog-logging-and-appsettings-3d4n

// First build the configuration object.
var builder = new ConfigurationBuilder();
var config = builder.AddJsonFile("consoleSettings.json").Build();

// Sign up for the unhandled exception which forces the flush.
AppDomain.CurrentDomain.UnhandledException += AppUnhandledException;

// Specifying the configuration for serilog - using forces a flush.
using var logger = LoggerComposition.BuildLogger(config, "ErmasMachineLearningPredictor");
logger.Information("{AppName} is starting.", System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);

try
{
    var host = Host.CreateDefaultBuilder() 
        .ConfigureServices((context, services) =>
        { // Adding the DI container for configuration

        })
        .UseSerilog(dispose: true) 
        .Build(); 
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}

return;

// Create the retriever.
//var rco = new RestClientOptions(config["BaseWeatherUri"]!);
//var retrieveObj = new RetrieveWeather(new RestClient(rco), new RestRequest());
//var loggingRetriever = new LoggingDecoratorRetrieveWeather(retrieveObj, logger);

static void AppUnhandledException(object sender, UnhandledExceptionEventArgs e)
{
    if (e.ExceptionObject is not Exception exception) return;
    Log.Logger.Error(exception, "Console application crashed");
    if (e.IsTerminating)
    {
        Log.CloseAndFlush();
    }
}