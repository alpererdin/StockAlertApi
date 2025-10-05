using Microsoft.AspNetCore.Mvc;
using StockAlertApi.Core.Enums;
using StockAlertApi.Core.Interfaces.Services;

namespace StockAlertApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlertsController : ControllerBase
{
    private readonly IAlertsService _alertsService;

    public AlertsController(IAlertsService alertsService)
    {
        _alertsService = alertsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserAlerts([FromQuery] Guid userId)
    {
        var alerts = await _alertsService.GetUserAlertsAsync(userId);
        return Ok(alerts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAlert(Guid id)
    {
        var alert = await _alertsService.GetAlertByIdAsync(id);
        if (alert == null)
            return NotFound();

        return Ok(alert);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAlert([FromBody] CreateAlertRequest request)
    {
        var alert = await _alertsService.CreateAlertAsync(
            request.UserId,
            request.StockId,
            request.TargetPrice,
            request.Direction
        );

        return CreatedAtAction(nameof(GetAlert), new { id = alert.Id }, alert);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAlert(Guid id, [FromQuery] Guid userId)
    {
        var success = await _alertsService.DeleteAlertAsync(id, userId);
        if (!success)
            return NotFound();

        return NoContent();
    }
}

public class CreateAlertRequest
{
    public Guid UserId { get; set; }
    public Guid StockId { get; set; }
    public decimal TargetPrice { get; set; }
    public AlertDirection Direction { get; set; }
}