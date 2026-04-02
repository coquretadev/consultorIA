using AiConsulting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AiConsulting.Infrastructure.Data.Configurations;

public class NotificationConfigConfiguration : IEntityTypeConfiguration<NotificationConfig>
{
    public void Configure(EntityTypeBuilder<NotificationConfig> builder)
    {
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Channel).IsRequired();
        builder.Property(n => n.WebhookUrl).HasMaxLength(500).IsRequired();
        builder.Property(n => n.IsActive).HasDefaultValue(false);
    }
}
