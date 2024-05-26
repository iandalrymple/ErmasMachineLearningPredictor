using Newtonsoft.Json;
using Predictor.Domain.Abstractions;
using Predictor.Domain.Exceptions;
using Predictor.RetrieveSalesApi.Models;
using RestSharp;

namespace Predictor.RetrieveSalesApi.Implementations;

public class RetrieveSales : IRetrieveSales
{
    private readonly string _encoded;
    private readonly RestClient _client;
    private readonly string _baseUri;
    private readonly Dictionary<string, string> _storeGuidLookUp;

    public RetrieveSales(string keyOne, string keyTwo, Dictionary<string, string> storeGuidLooksUp)
    {
        var combined = $"{keyOne}:{keyTwo}";
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(combined);
        _encoded = Convert.ToBase64String(plainTextBytes);

        _storeGuidLookUp = storeGuidLooksUp;
        _baseUri = @"https://pos-api.focuspos.com/api/checks/{business_date}";
        _client = new RestClient();
    }

    public async Task<decimal> Retrieve(DateTime dateTime, string storeName)
    {
        var request = new RestRequest(_baseUri)
            .AddUrlSegment("business_date", $"{dateTime.Month:d2}{dateTime.Day:d2}{dateTime.Year}");

        if (!_storeGuidLookUp.TryGetValue(storeName.ToUpper(), out var storeGuid))
        {
            throw new ArgumentNullException(nameof(storeName));
        }

        request.AddHeader("focuspos-restaurant-id", storeGuid);
        request.AddHeader("Accept", "application/json");
        request.AddHeader("Authorization", $"Basic {_encoded}");

        var result = await _client.ExecuteAsync(request);
        result.ThrowIfError();
        if (result.Content == null)
        {
            throw new NoSalesDataFromApiException(dateTime, storeName);
        }

        // Uncomment to write files for testing.
        //await File.WriteAllTextAsync("CheckListModelExample.json", result.Content);

        // Parse the model.
        var deserialized = JsonConvert.DeserializeObject<List<Root>>(result.Content) ?? throw new NoSalesDataFromApiException(dateTime, storeName);

        // Sum the total and the void.
        var salesTotal = deserialized.Sum(check => check.total);
        var voidTotal = deserialized.Sum(check => check.void_total);

        return Convert.ToDecimal(salesTotal - voidTotal);
    }
}