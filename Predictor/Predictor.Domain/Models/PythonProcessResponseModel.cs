using Newtonsoft.Json;

namespace Predictor.Domain.Models;

public class PythonProcessResponseModel
{
    [JsonProperty("error")]
    public int Error { get; init; }

    [JsonProperty("prediction")]
    public decimal Prediction { get; init; }

    [JsonProperty("logs")]
    public string[] Logs { get; init; } = [];
}