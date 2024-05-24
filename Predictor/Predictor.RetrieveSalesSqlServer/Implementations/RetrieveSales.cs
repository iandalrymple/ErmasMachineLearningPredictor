using Predictor.Domain.Abstractions;
using System.Data.SqlClient;

namespace Predictor.RetrieveSalesSqlServer.Implementations;

public class RetrieveSales : IRetrieveSales
{
    private readonly string _connectionString;

    public RetrieveSales(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public async Task<decimal> Retrieve(DateTime dateTime)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
    }
}