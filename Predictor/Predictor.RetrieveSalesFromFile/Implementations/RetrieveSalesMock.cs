using Predictor.Domain.Abstractions;

namespace Predictor.RetrieveSalesApi.Implementations;

public class RetrieveSalesMock : IRetrieveSales
{
    public Task<decimal> Retrieve(DateTime dateTime, string storeName)
    {
        var someDouble = Random.Shared.NextDouble();
        return Task.FromResult(Convert.ToDecimal(someDouble));
    }
}