using Microsoft.Extensions.Configuration;
using StockAlertApi.Core.Interfaces.Services;
using System.Text.Json;

namespace StockAlertApi.Infrastructure.ExternalApis;

public class FinnhubService : IFinanceApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public FinnhubService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["Finnhub:ApiKey"] ?? throw new InvalidOperationException("Finnhub API key not configured");
        _httpClient.BaseAddress = new Uri(configuration["Finnhub:BaseUrl"] ?? "https://finnhub.io/api/v1/");
    }

    public async Task<decimal?> GetCurrentPriceAsync(string tickerSymbol)
    {
        try
        {
            var response = await _httpClient.GetAsync($"quote?symbol={tickerSymbol}&token={_apiKey}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var data = FinnhubQuote.Deserialize(json);
            return data?.CurrentPrice;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error parsing {tickerSymbol}: {ex.Message}");
            return null;
        }
    }
    public async Task<Dictionary<string, decimal>> GetBatchPricesAsync(IEnumerable<string> tickerSymbols)
    {
        var prices = new Dictionary<string, decimal>();

        foreach (var symbol in tickerSymbols)
        {
            var price = await GetCurrentPriceAsync(symbol);
            if (price.HasValue)
            {
                prices[symbol] = price.Value;
            }
            await Task.Delay(100);
        }

        return prices;
    }
}

public class FinnhubQuote
{
    public decimal CurrentPrice { get; set; }
    public decimal HighPrice { get; set; }
    public decimal LowPrice { get; set; }
    public decimal OpenPrice { get; set; }
    public decimal PreviousClosePrice { get; set; }

    public static FinnhubQuote? Deserialize(string json)
    {
        var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        return new FinnhubQuote
        {
            CurrentPrice = root.GetProperty("c").GetDecimal(),
            HighPrice = root.GetProperty("h").GetDecimal(),
            LowPrice = root.GetProperty("l").GetDecimal(),
            OpenPrice = root.GetProperty("o").GetDecimal(),
            PreviousClosePrice = root.GetProperty("pc").GetDecimal()
        };
    }
}