using AiConsulting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AiConsulting.Infrastructure.Data.Configurations;

public class ServiceTranslationConfiguration : IEntityTypeConfiguration<ServiceTranslation>
{
    public void Configure(EntityTypeBuilder<ServiceTranslation> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.LanguageCode).HasMaxLength(10).IsRequired();
        builder.Property(t => t.Name).HasMaxLength(200).IsRequired();
        builder.Property(t => t.Description).HasMaxLength(2000).IsRequired();
        builder.Property(t => t.Benefits).HasMaxLength(2000).IsRequired();
        builder.Property(t => t.MetaTitle).HasMaxLength(200);
        builder.Property(t => t.MetaDescription).HasMaxLength(500);

        builder.HasIndex(t => new { t.ServiceId, t.LanguageCode }).IsUnique();

        builder.HasOne(t => t.Service)
            .WithMany(s => s.Translations)
            .HasForeignKey(t => t.ServiceId)
            .IsRequired();
    }
}
