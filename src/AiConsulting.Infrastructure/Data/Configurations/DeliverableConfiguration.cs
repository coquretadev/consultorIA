using AiConsulting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AiConsulting.Infrastructure.Data.Configurations;

public class DeliverableConfiguration : IEntityTypeConfiguration<Deliverable>
{
    public void Configure(EntityTypeBuilder<Deliverable> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Name).HasMaxLength(300).IsRequired();
        builder.Property(d => d.Description).HasMaxLength(1000);
        builder.Property(d => d.EstimatedHours).HasPrecision(18, 2);
        builder.Property(d => d.IsCompleted).HasDefaultValue(false);

        builder.HasOne(d => d.Project)
            .WithMany(p => p.Deliverables)
            .HasForeignKey(d => d.ProjectId)
            .IsRequired();

        builder.HasMany(d => d.TimeEntries)
            .WithOne(te => te.Deliverable)
            .HasForeignKey(te => te.DeliverableId);
    }
}
