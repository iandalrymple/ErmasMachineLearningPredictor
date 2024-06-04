using Predictor.Domain.Abstractions;
using Predictor.Domain.Models;

namespace Predictor.PredictingEnginePython.Implementations;

public class PredictingEnginePythonImpl : IPredictingEngine, IDisposable
{
    private bool _disposedValue;

    public Task<PredictingEngineResponseModel> RunProcessAsync(PredictingEngineParameterModel parameterModel)
    {
        throw new NotImplementedException();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposedValue) return;
        if (disposing)
        {
            // TODO: dispose managed state (managed objects)
        }

        // TODO: free unmanaged resources (unmanaged objects) and override finalizer
        // TODO: set large fields to null
        _disposedValue = true;
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~PredictingEnginePython()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}