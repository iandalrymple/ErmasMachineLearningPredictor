namespace Predictor.Domain.Models;

public class PredictingEngineParameterModel
{
    public required string StoreName { get; init; }
    public required string FeaturesPath { get; init; }

    public static async Task<string> CreateTempFile(string content)
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"Features_{Guid.NewGuid()}_{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}.csv");
        await File.WriteAllTextAsync(tempFile, content);
        return tempFile;
    }
}