using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Predictor.Console.Composition
{
    internal class LoggerComposition
    {
        internal static (Microsoft.Extensions.Logging.ILogger logger, ILoggerFactory factory) BuildLogger(IConfiguration config, string loggingFolderName)
        {
            // Switch that can be used by seq to control the logging programmatically.
            // https://nblumhardt.com/2014/10/dynamically-changing-the-serilog-level/
            var logLevel = config.GetSection("Logging").GetSection("LogLevel")["Serilog"];
            var levelSwitch = TranslateLogLevel(logLevel);

            // Configure the logger with console 
            var appDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), loggingFolderName);
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
            var serilog = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .WriteTo.File(logFileName, rollOnFileSizeLimit: true, fileSizeLimitBytes: 1048576)
#if DEBUG
                .WriteTo.Console()
#endif
                .WriteTo.Seq("http://localhost:5341",
                    apiKey: config["SeqApiKey"],
                    controlLevelSwitch: levelSwitch)
                .CreateLogger();
            Log.Logger = serilog;

            // This creates the MS ILogger 
            // https://stackoverflow.com/questions/59681394/correctly-injecting-serilog-into-net-core-classes-as-microsoft-extentions-loggi
            var loggerFactory = new LoggerFactory()
                .AddSerilog(serilog);
            var msLogger = loggerFactory.CreateLogger("Logger");
            return (msLogger, loggerFactory);
        }

        private static LoggingLevelSwitch TranslateLogLevel(string? logLevel)
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
    }
}
