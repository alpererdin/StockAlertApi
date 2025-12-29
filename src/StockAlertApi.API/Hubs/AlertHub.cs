using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace StockAlertApi.API.Hubs;

public class AlertHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        //var userId = Context.GetHttpContext()?.Request.Query["userId"].ToString();
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // var userId = Context.GetHttpContext()?.Request.Query["userId"].ToString();

        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        }

        await base.OnDisconnectedAsync(exception);
    }
}