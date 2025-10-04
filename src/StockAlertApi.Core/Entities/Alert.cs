using StockAlertApi.Core.Entities.Common;
using StockAlertApi.Core.Enums;

namespace StockAlertApi.Core.Entities;

public class Alert : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid StockId { get; set; }
    public decimal TargetPrice { get; set; }
    public AlertDirection Direction { get; set; }
    public AlertStatus Status { get; set; } = AlertStatus.Active;
    public DateTime? TriggeredAt { get; set; }
    public decimal? TriggerPrice { get; set; }
    public DateTime? LastCheckedAt { get; set; }
    public User User { get; set; } = null!;
    public Stock Stock { get; set; } = null!;
}