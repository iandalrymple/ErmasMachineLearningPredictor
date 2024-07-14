using Predictor.Domain.Abstractions;
using Predictor.Domain.Exceptions;
using Predictor.Domain.Models;
using Predictor.Domain.Models.StateModels;
using System.Data.SQLite;
using Dapper;
using Newtonsoft.Json;

namespace Predictor.RetrieveOwmWeatherSqlite.Implementations
{
    public class RetrieveWeather : IRetrieveWeather
    {
        private readonly string _connectionString;
        private readonly SQLiteConnection _connection;

        public RetrieveWeather(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new SQLiteConnection(connectionString);
        }

        public async Task<WeatherSourceModel?> Retrieve(WeatherRetrieveParamModel inParams)
        {
            await _connection.OpenAsync();
            const string queryString = "SELECT * FROM Weather WHERE Longitude=@Longitude AND Latitude=@Latitude AND DateTime=@DateTime;";
            var queryParams = new
            {
                inParams.Longitude,
                inParams.Latitude,   
                DateTime = inParams.DateTime.ToString("yyyy-MM-dd HH:mm:ss")
            };

            var result = (await _connection.QueryAsync<WeatherCacheModel>(queryString, queryParams)).ToList();
            await _connection.CloseAsync();

            if (result.Count == 0)
            {
                throw new WeatherDataNotFoundException(inParams.DateTime);
            }
            if (result.Count > 1)
            {
                throw new MoreThanOneRecordException($"Latitude => {inParams.Latitude} / Longitude => {inParams.Longitude} / DateTime => {inParams.DateTime}");
            }

            var firstRecord = result[0];
            if (string.IsNullOrWhiteSpace(firstRecord.WeatherJson))
            {
                return null;
            }

            // Create some settings for null handling 
            // https://stackoverflow.com/questions/31813055/how-to-handle-null-empty-values-in-jsonconvert-deserializeobject
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            var parsedResponse = JsonConvert.DeserializeObject<WeatherSourceModel>(firstRecord.WeatherJson, settings);
            return parsedResponse;
        }
    }
}
