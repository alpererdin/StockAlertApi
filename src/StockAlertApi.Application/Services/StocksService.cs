using StockAlertApi.Core.Entities;
using StockAlertApi.Core.Interfaces.Repositories;
using StockAlertApi.Core.Interfaces.Services;

namespace StockAlertApi.Application.Services;

public class StocksService : IStocksService
{
    private readonly IStockRepository _stockRepository;

    public StocksService(IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }

    public async Task<Stock?> GetOrCreateStockAsync(string tickerSymbol, string companyName)
    {
        var stock = await _stockRepository.GetByTickerSymbolAsync(tickerSymbol);

        if (stock == null)
        {
            stock = new Stock
            {
                TickerSymbol = tickerSymbol.ToUpper(),
                CompanyName = companyName
            };
            await _stockRepository.AddAsync(stock);
        }

        return stock;
    }

    public Task<Stock?> GetByTickerSymbolAsync(string tickerSymbol)
    {
        return _stockRepository.GetByTickerSymbolAsync(tickerSymbol);
    }

    public Task<List<Stock>> SearchAsync(string query)
    {
        return _stockRepository.SearchAsync(query);
    }

    public async Task UpdatePriceAsync(Guid stockId, decimal price)
    {
        var stock = await _stockRepository.GetByIdAsync(stockId);
        if (stock == null) return;

        stock.CurrentPrice = price;
        stock.LastPriceUpdate = DateTime.UtcNow;
        await _stockRepository.UpdateAsync(stock);
    }
}