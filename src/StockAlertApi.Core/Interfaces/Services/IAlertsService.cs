using StockAlertApi.Core.Entities;
using StockAlertApi.Core.Enums;

namespace StockAlertApi.Core.Interfaces.Services;

public interface IAlertsService
{
    Task<Alert?> CreateAlertAsync(Guid userId, Guid stockId, decimal targetPrice, AlertDirection direction);
    Task<List<Alert>> GetUserAlertsAsync(Guid userId);
    Task<Alert?> GetAlertByIdAsync(Guid id);
    Task<bool> DeleteAlertAsync(Guid id, Guid userId);
    Task<List<Alert>> GetActiveAlertsAsync();
    Task UpdateAlertStatusAsync(Guid alertId, AlertStatus status, decimal? triggerPrice = null);

    Task UpdateLastCheckedAsync(Guid alertId);
}