using Microsoft.AspNetCore.SignalR;
using StockAlertApi.API.Hubs;
using StockAlertApi.Core.Interfaces.Services;

namespace StockAlertApi.API.Services;

public class NotificationService : INotificationService
{
    private readonly IHubContext<AlertHub> _hubContext;

    public NotificationService(IHubContext<AlertHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendAlertTriggeredAsync(Guid userId, string stockSymbol, decimal currentPrice, decimal targetPrice)
    {
        await _hubContext.Clients.Group(userId.ToString()).SendAsync("AlertTriggered", new
        {
            StockSymbol = stockSymbol,
            CurrentPrice = currentPrice,
            TargetPrice = targetPrice,
            TriggeredAt = DateTime.UtcNow
        });
    }
}