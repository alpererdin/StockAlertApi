using Microsoft.EntityFrameworkCore;
using StockAlertApi.Core.Entities;
using StockAlertApi.Core.Enums;
using StockAlertApi.Core.Interfaces.Repositories;

namespace StockAlertApi.Infrastructure.Data.Repositories;

public class AlertRepository : IAlertRepository
{
    private readonly AppDbContext _context;

    public AlertRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Alert?> GetByIdAsync(Guid id)
    {
        return await _context.Alerts
            .Include(a => a.User)
            .Include(a => a.Stock)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<Alert>> GetUserAlertsAsync(Guid userId)
    {
        return await _context.Alerts
            .Include(a => a.Stock)
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Alert>> GetActiveAlertsAsync()
    {
        return await _context.Alerts
            .Include(a => a.Stock)
            .Include(a => a.User)
            .Where(a => a.Status == AlertStatus.Active)
            .ToListAsync();
    }

    public async Task AddAsync(Alert alert)
    {
        await _context.Alerts.AddAsync(alert);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Alert alert)
    {
        alert.UpdatedAt = DateTime.UtcNow;
        _context.Alerts.Update(alert);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var alert = await _context.Alerts.FindAsync(id);
        if (alert != null)
        {
            _context.Alerts.Remove(alert);
            await _context.SaveChangesAsync();
        }
    }
}