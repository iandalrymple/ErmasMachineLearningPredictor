using Predictor.Domain.Abstractions;
using Predictor.Domain.Models;

namespace Predictor.Testing.Mocks
{
    public class InsertSalesAlwaysTrueMock : ISalesInsert<CacheModel>
    {
        public Task<bool> Insert(CacheModel insertionData)
        {
            return Task.FromResult<bool>(true);
        }
    }
}
