using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockAlertApi.Core.Enums;
using StockAlertApi.Core.Interfaces.Services;
using System.Security.Claims;

namespace StockAlertApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AlertsController : ControllerBase
{
    private readonly IAlertsService _alertsService;

    public AlertsController(IAlertsService alertsService)
    {
        _alertsService = alertsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserAlerts()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
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
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var alert = await _alertsService.CreateAlertAsync(
            userId,
            request.StockId,
            request.TargetPrice,
            request.Direction
        );

        if (alert == null)
            return BadRequest(new { message = "Alert with same parameters already exists." });

        return CreatedAtAction(nameof(GetAlert), new { id = alert.Id }, alert);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAlert(Guid id)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var success = await _alertsService.DeleteAlertAsync(id, userId);
        if (!success)
            return NotFound();

        return NoContent();
    }
}

public class CreateAlertRequest
{
    public Guid StockId { get; set; }
    public decimal TargetPrice { get; set; }
    public AlertDirection Direction { get; set; }
}