using AiConsulting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AiConsulting.Infrastructure.Data.Configurations;

public class TopicNoteConfiguration : IEntityTypeConfiguration<TopicNote>
{
    public void Configure(EntityTypeBuilder<TopicNote> builder)
    {
        builder.HasKey(tn => tn.Id);

        builder.Property(tn => tn.Content).HasMaxLength(2000).IsRequired();
        builder.Property(tn => tn.ResourceUrl).HasMaxLength(500);

        builder.HasOne(tn => tn.Topic)
            .WithMany(t => t.Notes)
            .HasForeignKey(tn => tn.TopicId)
            .IsRequired();
    }
}
