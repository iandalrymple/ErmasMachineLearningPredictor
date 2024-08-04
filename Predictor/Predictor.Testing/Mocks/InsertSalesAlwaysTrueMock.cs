using Predictor.Domain.Abstractions;
using Predictor.Domain.Models;

namespace Predictor.Testing.Mocks
{
    public class InsertSalesAlwaysTrueMock : IGenericInsert<SalesCacheModel>
    {
        public Task<bool> Insert(SalesCacheModel insertionData)
        {
            return Task.FromResult<bool>(true);
        }
    }
}
