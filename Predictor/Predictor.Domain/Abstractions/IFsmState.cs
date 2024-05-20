using Predictor.Domain.Models;
using Predictor.Domain.System;

namespace Predictor.Domain.Abstractions;

public interface IFsmState
{
    PredictorFsmStates State { get; init; }

    Type ResultType { get; init; }

    Task Execute(FsmStatefulContainer container);
}