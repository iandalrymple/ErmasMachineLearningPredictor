using Predictor.Domain.Abstractions;

namespace Predictor.RetrieveSales.Implementations;

public class RetrieveSalesMock : IRetrieveSales
{
    public Task<decimal> Retrieve(DateTime dateTime)
    {
        var someDouble = Random.Shared.NextDouble();
        return Task.FromResult(Convert.ToDecimal(someDouble));
    }
}