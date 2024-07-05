using Predictor.Domain.Abstractions;
using Predictor.Domain.Models.StateModels;

namespace Predictor.Testing.Mocks;

public class RetrieveSalesReturnRandomMock : IRetrieveSales<decimal>
{
    public Task<decimal> Retrieve(DateTime dateTime, string storeName)
    {
        var someDouble = Random.Shared.NextDouble();
        return Task.FromResult(Convert.ToDecimal(someDouble));
    }
}

public class RetrieveSalesAlwaysNullMock : IRetrieveSales<StateCurrentSalesResultModel?>
{
    public async Task<StateCurrentSalesResultModel?> Retrieve(DateTime dateTime, string storeName)
    {
        await Task.Delay(10);
        return null;
    }
}