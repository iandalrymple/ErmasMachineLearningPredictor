namespace Predictor.Domain.Abstractions;

public interface IRetrieveSales<T>
{
    public Task<T> Retrieve(DateTime dateTime, string storeName);
}