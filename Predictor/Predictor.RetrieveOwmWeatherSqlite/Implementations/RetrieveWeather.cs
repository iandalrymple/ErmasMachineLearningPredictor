using System.Data.SQLite;
using Predictor.Domain.Abstractions;
using Predictor.Domain.Models;

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

        public async Task<WeatherSourceModel> Retrieve(WeatherRetrieveParamModel inParams)
        {
            throw new NotImplementedException();
        }
    }
}
