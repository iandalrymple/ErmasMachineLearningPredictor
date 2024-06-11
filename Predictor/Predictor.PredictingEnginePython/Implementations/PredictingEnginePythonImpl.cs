using Microsoft.Extensions.Logging;
using Predictor.Domain.Abstractions;
using Predictor.Domain.Models;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using Predictor.Domain.Exceptions;
using Predictor.Domain.Extensions;

namespace Predictor.PredictingEnginePython.Implementations;

public class PredictingEnginePythonImpl : IPredictingEngine, IDisposable
{
    public static readonly int PythonScriptFileIndex = 0;
    public static readonly int UticaModelPathIndexInArgs = PythonScriptFileIndex + 1;
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
        string modelPathFromArgs;
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

        // NOTE - the script file is the first argument always (main.py)
        var startInfo = new ProcessStartInfo
        {
            FileName = _pythonPath,
            Arguments = string.Join(" ", _args[PythonScriptFileIndex], modelPathFromArgs, _args[TransformerIndexInArgs], parameterModel.FeaturesPath),
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
            tcs.SetResult(BuildModel(process.ExitCode, rawConsoleStandard, rawConsoleError, guid, startTime, parameterModel.StoreName));

            // Clean up.
            if (File.Exists(parameterModel.FeaturesPath))
            {
                File.Delete(parameterModel.FeaturesPath);
            }
            process.Dispose();
            _processing = false;
            _processDictionary.TryRemove(guid, out _);
        };

        _ = process.Start();
        return tcs.Task;
    }

    private static PredictingEngineResponseModel BuildModel(
        int exitCode,
        string consoleResponse,
        string consoleError,
        Guid guid,
        DateTime startTime,
        string store)
    {
        try
        {
            var parsedString = consoleResponse.Between("!!!!!", "!!!!!").Replace("\\", "\\\\");
            var parsedModel = JsonConvert.DeserializeObject<PythonProcessResponseModel>(parsedString);
            var returnModel = new PredictingEngineResponseModel
            {
                ExitCode = exitCode,
                RawModelFromStandardInput = consoleResponse,
                ParsedModelFromStandardInput = parsedModel,
                RawStandardError = consoleError,
                Guid = guid,
                StartTime = startTime,
                EndTime = DateTime.Now,
                StoreName = store
            };
            return returnModel;
        }
        catch
        {
            // Intentionally gobbling
        }

        return new PredictingEngineResponseModel
        {
            ExitCode = exitCode,
            RawModelFromStandardInput = $"{consoleResponse} AND UNABLE TO PARSE MODEL",
            ParsedModelFromStandardInput = null,
            RawStandardError = consoleError,
            Guid = guid,
            StartTime = startTime,
            EndTime = DateTime.Now,
            StoreName = store
        };
    }

    public void CancelAllProcesses()
    {
        foreach (var p in _processDictionary)
        {
            p.Value.Kill();
            p.Value.Dispose();
        }
        _processDictionary.Clear();
        _processing = false;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposedValue) return;

        if (disposing)
        {
            CancelAllProcesses();
        }

        _disposedValue = true;
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}