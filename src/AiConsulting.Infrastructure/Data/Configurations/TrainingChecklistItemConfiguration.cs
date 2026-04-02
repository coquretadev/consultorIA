using AiConsulting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AiConsulting.Infrastructure.Data.Configurations;

public class TrainingChecklistItemConfiguration : IEntityTypeConfiguration<TrainingChecklistItem>
{
    public void Configure(EntityTypeBuilder<TrainingChecklistItem> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name).HasMaxLength(200).IsRequired();
        builder.Property(t => t.Description).HasMaxLength(1000);
        builder.Property(t => t.IsCompleted).HasDefaultValue(false);

        builder.HasIndex(t => t.SortOrder);
    }
}
