using Microsoft.Extensions.Logging;
using Predictor.Domain.Abstractions;
using Predictor.Domain.Models;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using Predictor.Domain.Exceptions;

namespace Predictor.PredictingEnginePython.Implementations;

public class PredictingEnginePythonImpl : IPredictingEngine, IDisposable
{
    public static readonly int UticaModelPathIndexInArgs = 0;
    private static readonly int WarrenModelPathIndexInArgs = UticaModelPathIndexInArgs + 1;
    private static readonly int TransformerIndexInArgs = WarrenModelPathIndexInArgs + 1;

    private readonly string _pythonPath;
    private readonly string _workingDirectory;
    private readonly string[] _args;
    private readonly ConcurrentDictionary<Guid, Process> _processDictionary;
    private readonly ILogger<PredictingEnginePythonImpl> _logger;

    private bool _processing;
    private bool _disposedValue;

    public bool Processing => _processing;
    public bool DoneProcessing => !_processing;

    public PredictingEnginePythonImpl(
        string pythonPath, 
        string workingDirectory, 
        string[] args, 
        ILogger<PredictingEnginePythonImpl> logger)
    {
        _pythonPath = pythonPath;
        _workingDirectory = workingDirectory;
        _args = args;
        _processDictionary = new ConcurrentDictionary<Guid, Process>();
        _logger = logger;
    }

    public Task<PredictingEngineResponseModel> PredictAsync(PredictingEngineParameterModel parameterModel)
    {
        _processing = true;
        var tcs = new TaskCompletionSource<PredictingEngineResponseModel>();

        // Get the path to the model based on the store name being passed in. 
        var modelPathFromArgs = string.Empty;
        if (parameterModel.StoreName.Equals("Utica", StringComparison.OrdinalIgnoreCase))
        {
            modelPathFromArgs = _args[UticaModelPathIndexInArgs];
        }
        else if(parameterModel.StoreName.Equals("Warren", StringComparison.OrdinalIgnoreCase))
        {
            modelPathFromArgs = _args[WarrenModelPathIndexInArgs];
        }
        else
        {
            throw new PredictionModelNotFoundException(nameof(parameterModel.StoreName));
        }

        var startInfo = new ProcessStartInfo
        {
            FileName = _pythonPath,
            Arguments = string.Join(" ", modelPathFromArgs, _args[TransformerIndexInArgs], parameterModel.Features),
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            WorkingDirectory = _workingDirectory
        };
        var process = new Process
        {
            StartInfo = startInfo,
            EnableRaisingEvents = true
        };
        var guid = Guid.NewGuid();
        var startTime = DateTime.Now;
        _processDictionary.TryAdd(guid, process);

        process.Exited += (sender, args) =>
        {
            // Populate the result. 
            using var standardReader = process.StandardOutput;
            var rawConsoleStandard = standardReader.ReadToEnd();
            using var errorReader = process.StandardError;
            var rawConsoleError = errorReader.ReadToEnd();
            if (string.IsNullOrEmpty(rawConsoleError) == false)
            {
                _logger.LogError("Error encountered in Python process {exception}", rawConsoleError);
            }
            tcs.SetResult(BuildModel(process.ExitCode, rawConsoleStandard, rawConsoleError, guid, startTime, storeName));

            // Clean up.
            process.Dispose();
            _processing = false;
            _processDictionary.TryRemove(guid, out _);
        };

        _ = process.Start();
        return tcs.Task;
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