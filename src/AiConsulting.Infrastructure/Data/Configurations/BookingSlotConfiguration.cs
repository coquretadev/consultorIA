using AiConsulting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AiConsulting.Infrastructure.Data.Configurations;

public class BookingSlotConfiguration : IEntityTypeConfiguration<BookingSlot>
{
    public void Configure(EntityTypeBuilder<BookingSlot> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.VisitorName).HasMaxLength(200).IsRequired();
        builder.Property(b => b.VisitorEmail).HasMaxLength(200).IsRequired();
        builder.Property(b => b.VisitorCompany).HasMaxLength(200).IsRequired();
        builder.Property(b => b.IsConfirmed).HasDefaultValue(false);

        // Unique constraint para prevenir double-booking concurrente
        builder.HasIndex(b => new { b.Date, b.StartTime })
            .IsUnique()
            .HasFilter("\"IsConfirmed\" = true");  // solo slots confirmados son únicos
    }
}
