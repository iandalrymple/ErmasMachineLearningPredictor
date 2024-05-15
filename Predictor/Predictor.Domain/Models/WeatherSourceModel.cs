using Newtonsoft.Json;

namespace Predictor.Domain.Models;

public class WeatherSourceModel
{
    [JsonProperty("lat")] public float Lat { get; set; }
    [JsonProperty("lon")] public float Lon { get; set; }
    [JsonProperty("timezone")] public string Timezone { get; set; } = string.Empty;
    [JsonProperty("timezone_offset")] public int TimezoneOffset { get; set; }
    [JsonProperty("data")] public Datum[]? Data { get; set; }
}

public class Datum
{
    [JsonProperty("dt")] public long Dt { get; set; }
    [JsonProperty("sunrise")] public long Sunrise { get; set; }
    [JsonProperty("sunset")] public long Sunset { get; set; }
    [JsonProperty("temp")] public float Temp { get; set; }
    [JsonProperty("feels_like")] public float FeelsLike { get; set; }
    [JsonProperty("pressure")] public float Pressure { get; set; }
    [JsonProperty("humidity")] public float Humidity { get; set; }
    [JsonProperty("dew_point")] public float DewPoint { get; set; }
    [JsonProperty("uvi")] public float Uvi { get; set; }
    [JsonProperty("clouds")] public float Clouds { get; set; }
    [JsonProperty("visibility")] public float Visibility { get; set; }
    [JsonProperty("wind_speed")] public float WindSpeed { get; set; }
    [JsonProperty("wind_gust")] public float WindGust { get; set; }
    [JsonProperty("wind_deg")] public float WindDeg { get; set; }
    [JsonProperty("weather")] public Weather[]? Weather { get; set; }
}

public class Weather
{
    [JsonProperty("id")] public int Id { get; set; }
    [JsonProperty("main")] public string Main { get; set; } = string.Empty;
    [JsonProperty("description")] public string Description { get; set; } = string.Empty;
    [JsonProperty("icon")] public string Icon { get; set; } = string.Empty;
}