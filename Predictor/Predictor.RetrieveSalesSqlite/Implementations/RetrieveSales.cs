using Predictor.Domain.Abstractions;
using Predictor.Domain.Models.StateModels;

namespace Predictor.RetrieveSalesSqlite.Implementations
{
    public class RetrieveSales : IRetrieveSales<StateCurrentSalesResultModel?>
    {
        private readonly string _connectionString;

        public RetrieveSales(string connectionString)
        {
            _connectionString = connectionString;   
        }

        public Task<StateCurrentSalesResultModel?> Retrieve(DateTime dateTime, string storeName)
        {
            throw new NotImplementedException();
        }
    }
}
