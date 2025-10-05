namespace StockAlertApi.Core.Interfaces.Services;

public interface IFinanceApiService
{
    Task<decimal?> GetCurrentPriceAsync(string tickerSymbol);
    Task<Dictionary<string, decimal>> GetBatchPricesAsync(IEnumerable<string> tickerSymbols);
}