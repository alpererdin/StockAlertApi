using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockAlertApi.Core.Entities;

namespace StockAlertApi.Infrastructure.Data.Configurations;

public class AlertConfiguration : IEntityTypeConfiguration<Alert>
{
    public void Configure(EntityTypeBuilder<Alert> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.TargetPrice)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(a => a.TriggerPrice)
            .HasPrecision(18, 2);

        builder.Property(a => a.Direction)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(a => a.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.HasOne(a => a.User)
            .WithMany(u => u.Alerts)
            .HasForeignKey(a => a.UserId);

        builder.HasOne(a => a.Stock)
            .WithMany(s => s.Alerts)
            .HasForeignKey(a => a.StockId);

        builder.HasIndex(a => new { a.UserId, a.Status });
        builder.HasIndex(a => a.Status);
    }
}