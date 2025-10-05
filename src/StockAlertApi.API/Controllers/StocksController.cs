using Microsoft.AspNetCore.Mvc;
using StockAlertApi.Core.Interfaces.Services;

namespace StockAlertApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StocksController : ControllerBase
{
    private readonly IStocksService _stocksService;

    public StocksController(IStocksService stocksService)
    {
        _stocksService = stocksService;
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest("Query parameter is required");

        var stocks = await _stocksService.SearchAsync(query);
        return Ok(stocks);
    }

    [HttpGet("{tickerSymbol}")]
    public async Task<IActionResult> GetByTickerSymbol(string tickerSymbol)
    {
        var stock = await _stocksService.GetByTickerSymbolAsync(tickerSymbol);
        if (stock == null)
            return NotFound();

        return Ok(stock);
    }

    [HttpPost]
    public async Task<IActionResult> CreateStock([FromBody] CreateStockRequest request)
    {
        var stock = await _stocksService.GetOrCreateStockAsync(
            request.TickerSymbol,
            request.CompanyName
        );

        return Ok(stock);
    }
}

public class CreateStockRequest
{
    public string TickerSymbol { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
}