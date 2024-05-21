namespace Predictor.Domain.Abstractions;

public interface IRetrieveSales
{
    public Task<decimal> Retrieve(DateTime dateTime);
}