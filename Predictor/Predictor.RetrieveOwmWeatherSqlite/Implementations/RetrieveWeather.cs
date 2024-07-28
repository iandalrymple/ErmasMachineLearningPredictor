using Dapper;
using Newtonsoft.Json;
using Predictor.Domain.Abstractions;
using Predictor.Domain.Exceptions;
using Predictor.Domain.Models;
using System.Data.SQLite;

namespace Predictor.RetrieveOwmWeatherSqlite.Implementations
{
    public class RetrieveWeather : IRetrieveWeather
    {
        private readonly string _connectionString;
        private readonly SQLiteConnection _connection;
        private const double CoordinateBand = 0.001;

        public RetrieveWeather(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new SQLiteConnection(connectionString);
        }

        public async Task<WeatherSourceModel?> Retrieve(WeatherRetrieveParamModel inParams)
        {
            await _connection.OpenAsync();
            const string queryString = "SELECT * FROM Weather WHERE " +
                                       "Longitude>=@longitudeMin AND Longitude<=@longitudeMax AND " +
                                       "Latitude>=@latitudeMin AND Latitude<=@latitudeMax AND " +
                                       "DateTime=@DateTime;";
            var longitudeMin = inParams.Longitude - CoordinateBand;
            var longitudeMax = inParams.Longitude + CoordinateBand;
            var latitudeMin = inParams.Latitude - CoordinateBand;
            var latitudeMax = inParams.Latitude + CoordinateBand;   
            var queryParams = new
            {
                longitudeMin,
                longitudeMax,
                latitudeMin,
                latitudeMax,
                DateTime = inParams.DateTime.ToString("yyyy-MM-dd HH:mm:ss")
            };

            var result = (await _connection.QueryAsync<WeatherCacheModel>(queryString, queryParams)).ToList();
            await _connection.CloseAsync();

            switch (result.Count)
            {
                case 0:
                    throw new WeatherDataNotFoundException(inParams.DateTime);
                case > 1:
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


            // TODO - insert the lat and lon 
            sdfsdfsd

            return parsedResponse;
        }
    }
}
