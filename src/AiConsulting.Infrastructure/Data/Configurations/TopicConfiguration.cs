using AiConsulting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AiConsulting.Infrastructure.Data.Configurations;

public class TopicConfiguration : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name).HasMaxLength(200).IsRequired();
        builder.Property(t => t.Description).HasMaxLength(1000);
        builder.Property(t => t.IsCompleted).HasDefaultValue(false);

        builder.HasOne(t => t.TrainingWeek)
            .WithMany(tw => tw.Topics)
            .HasForeignKey(t => t.TrainingWeekId)
            .IsRequired();

        builder.HasMany(t => t.Notes)
            .WithOne(n => n.Topic)
            .HasForeignKey(n => n.TopicId);
    }
}
