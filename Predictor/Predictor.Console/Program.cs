using Microsoft.Extensions.Configuration;
using Predictor.RetrieveOwmWeather.Implementations;
using RestSharp;
using Serilog;

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("testSettings.json")
    .Build();

// NEED TO use serilog and cast to MS ILogger
var logger = new LoggerConfiguration()
    .WriteTo.File("AppLogs.log", rollOnFileSizeLimit: true, fileSizeLimitBytes: 1048576)
    .WriteTo.Console()
    .CreateLogger();

// Create the retriever.
var rco = new RestClientOptions(config["BaseWeatherUri"]!);
var retrieveObj = new RetrieveWeather(new RestClient(rco), new RestRequest());
var loggingRetriever = new LoggingDecoratorRetrieveWeather(retrieveObj, logger);

