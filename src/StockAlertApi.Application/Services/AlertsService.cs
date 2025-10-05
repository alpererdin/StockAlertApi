using StockAlertApi.Core.Entities;
using StockAlertApi.Core.Enums;
using StockAlertApi.Core.Interfaces.Repositories;
using StockAlertApi.Core.Interfaces.Services;

namespace StockAlertApi.Application.Services;

public class AlertsService : IAlertsService
{
    private readonly IAlertRepository _alertRepository;

    public AlertsService(IAlertRepository alertRepository)
    {
        _alertRepository = alertRepository;
    }

    public async Task<Alert?> CreateAlertAsync(Guid userId, Guid stockId, decimal targetPrice, AlertDirection direction)
    {
        var existingAlert = (await _alertRepository.GetUserAlertsAsync(userId))
       .FirstOrDefault(a =>
           a.StockId == stockId &&
           a.TargetPrice == targetPrice &&
           a.Direction == direction &&
           a.Status == AlertStatus.Active);

        if (existingAlert != null)
        {
            return null;  
        }
        var alert = new Alert
        {
            UserId = userId,
            StockId = stockId,
            TargetPrice = targetPrice,
            Direction = direction,
            Status = AlertStatus.Active
        };

        await _alertRepository.AddAsync(alert);
        return alert;
    }

    public Task<List<Alert>> GetUserAlertsAsync(Guid userId)
    {
        return _alertRepository.GetUserAlertsAsync(userId);
    }

    public Task<Alert?> GetAlertByIdAsync(Guid id)
    {
        return _alertRepository.GetByIdAsync(id);
    }

    public async Task<bool> DeleteAlertAsync(Guid id, Guid userId)
    {
        var alert = await _alertRepository.GetByIdAsync(id);
        if (alert == null || alert.UserId != userId)
        {
            return false;
        }

        await _alertRepository.DeleteAsync(id);
        return true;
    }

    public Task<List<Alert>> GetActiveAlertsAsync()
    {
        return _alertRepository.GetActiveAlertsAsync();
    }

    public async Task UpdateAlertStatusAsync(Guid alertId, AlertStatus status, decimal? triggerPrice = null)
    {
        var alert = await _alertRepository.GetByIdAsync(alertId);
        if (alert == null) return;

        alert.Status = status;
        if (status == AlertStatus.Triggered)
        {
            alert.TriggeredAt = DateTime.UtcNow;
            alert.TriggerPrice = triggerPrice;
        }

        await _alertRepository.UpdateAsync(alert);
    }
}