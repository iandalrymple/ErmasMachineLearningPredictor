using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Predictor.Domain.Abstractions;
using Predictor.Domain.Models;
using Predictor.Domain.System;

namespace Predictor.Domain.Implementations;

public class FsmConductor : IFsmConductor
{
    private readonly ILogger<FsmConductor> _logger;
    private readonly ConcurrentDictionary<PredictorFsmStates, IFsmState> _states;

    public FsmConductor(
        ILogger<FsmConductor> logger, 
        ConcurrentDictionary<PredictorFsmStates, IFsmState> states,
        FsmStatefulContainer container)
    {
        _logger = logger;
        _states = states;

        StateContainer = container;
    }

    public FsmStatefulContainer StateContainer { get; }

    public async Task Execute()
    {
        while (StateContainer.CurrentState < PredictorFsmStates.Completed)
        {
            _logger.LogInformation("About to execute state {state}.", StateContainer.CurrentState);
            await _states[StateContainer.CurrentState].Execute(StateContainer);
            await Task.Delay(100);
        }
    }
}