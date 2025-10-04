using StockAlertApi.Core.Entities;
using StockAlertApi.Core.Enums;

namespace StockAlertApi.Core.Interfaces.Repositories;

public interface IAlertRepository
{
    Task<Alert?> GetByIdAsync(Guid id);
    Task<List<Alert>> GetUserAlertsAsync(Guid userId);
    Task<List<Alert>> GetActiveAlertsAsync();
    Task AddAsync(Alert alert);
    Task UpdateAsync(Alert alert);
    Task DeleteAsync(Guid id);
}