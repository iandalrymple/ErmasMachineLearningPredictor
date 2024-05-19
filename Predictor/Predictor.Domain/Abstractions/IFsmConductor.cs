using Predictor.Domain.Models;

namespace Predictor.Domain.Abstractions;

public interface IFsmConductor
{
    Task Execute();
    public FsmStatefulContainer StateContainer { get; }
}