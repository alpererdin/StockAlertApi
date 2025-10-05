namespace StockAlertApi.Core.Interfaces.Services;

public interface INotificationService
{
    Task SendAlertTriggeredAsync(Guid userId, string stockSymbol, decimal currentPrice, decimal targetPrice);
}