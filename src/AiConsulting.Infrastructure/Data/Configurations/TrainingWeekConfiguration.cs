using AiConsulting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AiConsulting.Infrastructure.Data.Configurations;

public class TrainingWeekConfiguration : IEntityTypeConfiguration<TrainingWeek>
{
    public void Configure(EntityTypeBuilder<TrainingWeek> builder)
    {
        builder.HasKey(tw => tw.Id);

        builder.Property(tw => tw.Title).HasMaxLength(200).IsRequired();
        builder.Property(tw => tw.Description).HasMaxLength(1000);

        builder.HasIndex(tw => tw.WeekNumber).IsUnique();

        builder.HasMany(tw => tw.Topics)
            .WithOne(t => t.TrainingWeek)
            .HasForeignKey(t => t.TrainingWeekId);
    }
}
