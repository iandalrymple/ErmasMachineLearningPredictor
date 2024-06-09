using Predictor.Domain.Models;
using Predictor.PredictingEnginePython.Implementations;

namespace Predictor.Testing.PredictingEnginePython;

public class TestPredictingEnginePython
{
    [Fact]
    public async Task TestRunProcessAsync()
    {
        // Arrange
        var sut = new PredictingEnginePythonImpl();
        var inputParams = new PredictingEngineParameterModel();

        // Act
        var result = await sut.PredictAsync(inputParams);

        // Assert
        Assert.NotNull(result);
    }
}