using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockAlertApi.Core.Entities;

namespace StockAlertApi.Infrastructure.Data.Configurations;

public class StockConfiguration : IEntityTypeConfiguration<Stock>
{
    public void Configure(EntityTypeBuilder<Stock> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.TickerSymbol)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(s => s.CompanyName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(s => s.CurrentPrice)
            .HasPrecision(18, 2);

        builder.HasIndex(s => s.TickerSymbol).IsUnique();

        builder.HasMany(s => s.Alerts)
            .WithOne(a => a.Stock)
            .HasForeignKey(a => a.StockId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}