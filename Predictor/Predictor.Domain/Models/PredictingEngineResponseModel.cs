namespace Predictor.Domain.Models;

public class PredictingEngineResponseModel
{
    public required int ExitCode { get; init; }
    public required string? RawModelFromStandardInput { get; init; }
    public required PythonProcessResponseModel? ParsedModelFromStandardInput { get; init; }
    public required string? RawStandardError { get; init; }
    public required Guid Guid { get; init; }
    public required DateTime StartTime { get; init; }
    public required DateTime EndTime { get; init; }
    public required string StoreName { get; init; }

    public string AggregatedLog
    {
        get
        {
            var returnString = string.Empty;
            returnString += $"{nameof(ExitCode)}={ExitCode} ";
            returnString += $"{nameof(Guid)}={Guid} ";
            returnString += $"{nameof(Error)}={Error} ";
            returnString += $"{nameof(StartTime)}={StartTime} ";
            returnString += $"{nameof(TotalTimeInSeconds)}={TotalTimeInSeconds} ";
            if (string.IsNullOrEmpty(RawStandardError) == false)
            {
                returnString += $"{nameof(RawStandardError)}={RawStandardError} ";
            }

            if (string.IsNullOrEmpty(RawModelFromStandardInput) == false)
            {
                returnString += $"{nameof(RawModelFromStandardInput)}={RawModelFromStandardInput}  ";
            }
            return returnString.TrimEnd();
        }
    }

    public string PrettyAggregatedLog
    {
        get
        {
            var returnString = string.Empty;
            returnString += $"{nameof(ExitCode)}={ExitCode}{Environment.NewLine}";
            returnString += $"{nameof(Guid)}={Guid}{Environment.NewLine}";
            returnString += $"{nameof(Error)}={Error}{Environment.NewLine}";
            returnString += $"{nameof(StartTime)}={StartTime}{Environment.NewLine}";
            returnString += $"{nameof(TotalTimeInSeconds)}={TotalTimeInSeconds}{Environment.NewLine}";
            returnString += $"{nameof(Logs)}={Logs}{Environment.NewLine}";
            if (string.IsNullOrEmpty(RawStandardError) == false)
            {
                returnString += $"{nameof(RawStandardError)}={RawStandardError}{Environment.NewLine}";
            }
            return returnString.TrimEnd('\r', '\n');
        }
    }

    public int TotalTimeInSeconds => Convert.ToInt32(EndTime.Subtract(StartTime).TotalSeconds);

    public string Error
    {
        get
        {
            if (ParsedModelFromStandardInput == null)
            {
                return "Unknown";
            }

            return ParsedModelFromStandardInput.Error == 0 ? "None" : "Yes";
        }
    }

    public string Logs
    {
        get
        {
            if (ParsedModelFromStandardInput == null || ParsedModelFromStandardInput.Logs.Length == 0)
            {
                return "NO LOGS FROM PYTHON PROCESS.";
            }

            var returnString = string.Empty;
            foreach (var log in ParsedModelFromStandardInput.Logs)
            {
                returnString += log + Environment.NewLine;
            }

            return returnString.TrimEnd(Environment.NewLine.ToCharArray());
        }
    }
}