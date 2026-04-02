using AiConsulting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AiConsulting.Infrastructure.Data.Configurations;

public class TimeEntryConfiguration : IEntityTypeConfiguration<TimeEntry>
{
    public void Configure(EntityTypeBuilder<TimeEntry> builder)
    {
        builder.HasKey(te => te.Id);

        builder.Property(te => te.Hours).HasPrecision(18, 2);
        builder.Property(te => te.Description).HasMaxLength(500);

        builder.HasOne(te => te.Deliverable)
            .WithMany(d => d.TimeEntries)
            .HasForeignKey(te => te.DeliverableId)
            .IsRequired();
    }
}
