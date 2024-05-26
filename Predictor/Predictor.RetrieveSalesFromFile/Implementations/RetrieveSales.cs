using Predictor.Domain.Abstractions;

namespace Predictor.RetrieveSalesApi.Implementations;

public class RetrieveSales : IRetrieveSales
{
    private readonly string _keyOne;
    private readonly string _keyTwo;

    public RetrieveSales(string keyOne, string keyTwo)
    {
        _keyOne = keyOne;
        _keyTwo = keyTwo;
    }

    public Task<decimal> Retrieve(DateTime dateTime, string storeName)
    {
        throw new NotImplementedException();
    }
}