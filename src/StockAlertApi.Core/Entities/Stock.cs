using StockAlertApi.Core.Entities.Common;

namespace StockAlertApi.Core.Entities;

public class Stock : BaseEntity
{
    public string TickerSymbol { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public decimal? CurrentPrice { get; set; }
    public DateTime? LastPriceUpdate { get; set; }
    public ICollection<Alert> Alerts { get; set; } = new List<Alert>();
}