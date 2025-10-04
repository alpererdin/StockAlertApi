using StockAlertApi.Core.Entities;

namespace StockAlertApi.Core.Interfaces.Services;

public interface IStocksService
{
    Task<Stock?> GetOrCreateStockAsync(string tickerSymbol, string companyName);
    Task<Stock?> GetByTickerSymbolAsync(string tickerSymbol);
    Task<List<Stock>> SearchAsync(string query);
    Task UpdatePriceAsync(Guid stockId, decimal price);
}