using AiConsulting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AiConsulting.Infrastructure.Data.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).HasMaxLength(300).IsRequired();
        builder.Property(p => p.TotalEstimatedHours).HasPrecision(18, 2);
        builder.Property(p => p.ProgressPercentage).HasPrecision(5, 2);
        builder.Property(p => p.Status).HasConversion<string>();

        builder.HasOne(p => p.Client)
            .WithMany(c => c.Projects)
            .HasForeignKey(p => p.ClientId)
            .IsRequired();

        builder.HasOne(p => p.Service)
            .WithMany(s => s.Projects)
            .HasForeignKey(p => p.ServiceId)
            .IsRequired();

        builder.HasMany(p => p.Deliverables)
            .WithOne(d => d.Project)
            .HasForeignKey(d => d.ProjectId);

        builder.HasMany(p => p.StatusChanges)
            .WithOne(sc => sc.Project)
            .HasForeignKey(sc => sc.ProjectId);
    }
}
