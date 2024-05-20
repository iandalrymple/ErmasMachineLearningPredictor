using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Predictor.Domain.Models;
using Predictor.RetrieveOwmWeather.Implementations;

namespace Predictor.Testing.Domain;

public class TestStateWeather
{
    private readonly IConfiguration _configuration;

    public TestStateWeather()
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("testSettings.json")
            .Build();
    }


    [Fact]
    public async Task TestExecute_Happy()
    {
        // Arrange


        // Act
        

        // Assert
       
    }
}