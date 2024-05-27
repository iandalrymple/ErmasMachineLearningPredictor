using Predictor.Domain.Abstractions;

namespace Predictor.Testing.Mocks;

public class RetrieveSalesMock : IRetrieveSales<decimal>
{
    public Task<decimal> Retrieve(DateTime dateTime, string storeName)
    {
        var someDouble = Random.Shared.NextDouble();
        return Task.FromResult(Convert.ToDecimal(someDouble));
    }
}