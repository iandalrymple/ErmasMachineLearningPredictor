using Predictor.Domain.Abstractions;
using Predictor.Domain.Models;

namespace Predictor.Testing.Mocks
{
    public class InsertSalesAlwaysTrueMock : ISalesInsert<SalesCacheModel>
    {
        public Task<bool> Insert(SalesCacheModel insertionData)
        {
            return Task.FromResult<bool>(true);
        }
    }
}
