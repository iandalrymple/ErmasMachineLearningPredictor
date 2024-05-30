using Newtonsoft.Json;

namespace Predictor.Domain.Models;

public class HolidaysModel
{
    [JsonProperty(PropertyName = "date")]
    public DateOnly Date { get; set; }

    [JsonProperty(PropertyName = "localName")]
    public string? LocalName { get; set; }

    [JsonProperty(PropertyName = "name")]
    public string? Name { get; set; }

    [JsonProperty(PropertyName = "countryCode")]
    public string? CountryCode { get; set; }

    [JsonProperty(PropertyName = "fixed")]
    public bool? Fixed { get; set; }

    [JsonProperty(PropertyName = "global")]
    public bool? Global { get; set; }

    [JsonProperty(PropertyName = "counties")]
    public List<string>? Counties { get; set; }

    [JsonProperty(PropertyName = "launchYear")]
    public int? LaunchYear { get; set; }

    [JsonProperty(PropertyName = "types")]
    public List<string>? Types { get; set; }
}