using StockAlertApi.Core.Entities;

namespace StockAlertApi.Core.Interfaces.Repositories;

public interface IStockRepository
{
    Task<Stock?> GetByIdAsync(Guid id);
    Task<Stock?> GetByTickerSymbolAsync(string tickerSymbol);
    Task<List<Stock>> SearchAsync(string query);
    Task AddAsync(Stock stock);
    Task UpdateAsync(Stock stock);
}