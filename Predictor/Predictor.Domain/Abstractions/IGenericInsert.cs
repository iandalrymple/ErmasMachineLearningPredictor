namespace Predictor.Domain.Abstractions;

public interface IGenericInsert<in T> where T : class
{
    public Task<bool> Insert(T insertionData);
}