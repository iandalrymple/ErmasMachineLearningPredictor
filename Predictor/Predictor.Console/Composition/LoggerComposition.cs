using Serilog.Core;
using Serilog.Events;

namespace Predictor.Console.Composition
{
    internal class LoggerComposition
    {
        internal static LoggingLevelSwitch TranslateLogLevel(string? logLevel)
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
