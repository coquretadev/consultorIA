using AiConsulting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AiConsulting.Infrastructure.Data.Configurations;

public class StatusChangeConfiguration : IEntityTypeConfiguration<StatusChange>
{
    public void Configure(EntityTypeBuilder<StatusChange> builder)
    {
        builder.HasKey(sc => sc.Id);

        builder.Property(sc => sc.FromStatus).HasConversion<string>();
        builder.Property(sc => sc.ToStatus).HasConversion<string>();

        builder.HasOne(sc => sc.Project)
            .WithMany(p => p.StatusChanges)
            .HasForeignKey(sc => sc.ProjectId)
            .IsRequired();
    }
}
