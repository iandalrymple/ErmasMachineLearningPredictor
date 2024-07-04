namespace Predictor.Domain.Abstractions;

public interface ISalesInsert<in T>
{
    public Task<bool> Insert(T insertionData);
}