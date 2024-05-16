using Microsoft.Extensions.Logging;
using Predictor.Console.Composition;
using Predictor.RetrieveOwmWeather.Implementations;
using RestSharp;

try
{
    // Create the config and the logger.
    var config = ConfigurationComposition.BuildConfiguration("consoleSettings.json");
    var logger = LoggerComposition.BuildLogger(config, "ErmasMachineLearningPredictor");
    logger.logger.LogInformation("{AppName} is starting.", System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
    //logger.factory.Dispose();
    await Task.Delay(2500);
    return;
}
catch
{
    
}
finally
{

}


return;
// Create the retriever.
//var rco = new RestClientOptions(config["BaseWeatherUri"]!);
//var retrieveObj = new RetrieveWeather(new RestClient(rco), new RestRequest());
//var loggingRetriever = new LoggingDecoratorRetrieveWeather(retrieveObj, logger);

