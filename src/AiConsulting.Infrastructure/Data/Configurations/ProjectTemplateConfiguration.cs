using AiConsulting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AiConsulting.Infrastructure.Data.Configurations;

public class ProjectTemplateConfiguration : IEntityTypeConfiguration<ProjectTemplate>
{
    public void Configure(EntityTypeBuilder<ProjectTemplate> builder)
    {
        builder.HasKey(pt => pt.Id);

        builder.Property(pt => pt.Name).HasMaxLength(300).IsRequired();
        builder.Property(pt => pt.EstimatedTotalHours).HasPrecision(18, 2);
        builder.Property(pt => pt.DefaultDeliverables).HasColumnType("jsonb");
        builder.Property(pt => pt.DefaultMilestones).HasColumnType("jsonb");

        builder.HasOne(pt => pt.Service)
            .WithMany()
            .HasForeignKey(pt => pt.ServiceId)
            .IsRequired();
    }
}
