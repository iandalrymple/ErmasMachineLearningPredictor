using Predictor.Domain.Abstractions;
using Predictor.Domain.Models;

namespace Predictor.InsertSalesSqlite.Implementations;

public class InsertSales : ISalesInsert<CacheModel>
{
    public Task<bool> Insert(CacheModel insertionData)
    {
        throw new NotImplementedException();
    }
}