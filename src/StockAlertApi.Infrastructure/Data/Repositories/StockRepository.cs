using Microsoft.EntityFrameworkCore;
using StockAlertApi.Core.Entities;
using StockAlertApi.Core.Interfaces.Repositories;

namespace StockAlertApi.Infrastructure.Data.Repositories;

public class StockRepository : IStockRepository
{
    private readonly AppDbContext _context;

    public StockRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Stock?> GetByIdAsync(Guid id)
    {
        return await _context.Stocks.FindAsync(id);
    }

    public async Task<Stock?> GetByTickerSymbolAsync(string tickerSymbol)
    {
        return await _context.Stocks
            .FirstOrDefaultAsync(s => s.TickerSymbol == tickerSymbol.ToUpper());
    }

    public async Task<List<Stock>> SearchAsync(string query)
    {
        return await _context.Stocks
            .Where(s => s.TickerSymbol.Contains(query.ToUpper()) ||
                       s.CompanyName.Contains(query))
            .Take(10)
            .ToListAsync();
    }

    public async Task AddAsync(Stock stock)
    {
        await _context.Stocks.AddAsync(stock);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Stock stock)
    {
        stock.UpdatedAt = DateTime.UtcNow;
        _context.Stocks.Update(stock);
        await _context.SaveChangesAsync();
    }
}