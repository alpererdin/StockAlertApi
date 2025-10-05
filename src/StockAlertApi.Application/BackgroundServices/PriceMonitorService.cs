using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StockAlertApi.Core.Enums;
using StockAlertApi.Core.Interfaces.Services;

namespace StockAlertApi.Application.BackgroundServices;

public class PriceMonitorService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PriceMonitorService> _logger;

    public PriceMonitorService(IServiceProvider serviceProvider, ILogger<PriceMonitorService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Price Monitor Service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CheckAlertsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Price Monitor Service");
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private async Task CheckAlertsAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var alertsService = scope.ServiceProvider.GetRequiredService<IAlertsService>();
        var stocksService = scope.ServiceProvider.GetRequiredService<IStocksService>();
        var financeApi = scope.ServiceProvider.GetRequiredService<IFinanceApiService>();
        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

        var activeAlerts = await alertsService.GetActiveAlertsAsync();
        if (!activeAlerts.Any())
        {
            _logger.LogInformation("No active alerts to check");
            return;
        }

        var tickerSymbols = activeAlerts.Select(a => a.Stock.TickerSymbol).Distinct();
        var prices = await financeApi.GetBatchPricesAsync(tickerSymbols);

        foreach (var alert in activeAlerts)
        {
            if (!prices.TryGetValue(alert.Stock.TickerSymbol, out var currentPrice))
                continue;

            await stocksService.UpdatePriceAsync(alert.StockId, currentPrice);

            var shouldTrigger = alert.Direction == AlertDirection.Above
                ? currentPrice >= alert.TargetPrice
                : currentPrice <= alert.TargetPrice;

            if (shouldTrigger)
            {
                await alertsService.UpdateAlertStatusAsync(alert.Id, AlertStatus.Triggered, currentPrice);

                await notificationService.SendAlertTriggeredAsync(
                    alert.UserId,
                    alert.Stock.TickerSymbol,
                    currentPrice,
                    alert.TargetPrice
                );

                _logger.LogInformation(
                    "Alert triggered and notification sent: {Symbol} reached {Price} (Target: {Target})",
                    alert.Stock.TickerSymbol, currentPrice, alert.TargetPrice);
            }

            alert.LastCheckedAt = DateTime.UtcNow;
        }
    }
}