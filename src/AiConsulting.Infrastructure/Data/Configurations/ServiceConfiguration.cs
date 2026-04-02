using AiConsulting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AiConsulting.Infrastructure.Data.Configurations;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name).HasMaxLength(200).IsRequired();
        builder.Property(s => s.Description).HasMaxLength(2000).IsRequired();
        builder.Property(s => s.Benefits).HasMaxLength(2000).IsRequired();
        builder.Property(s => s.TargetSector).HasMaxLength(500);
        builder.Property(s => s.PriceRangeMin).HasPrecision(18, 2);
        builder.Property(s => s.PriceRangeMax).HasPrecision(18, 2);
        builder.Property(s => s.Slug).HasMaxLength(200).IsRequired();
        builder.Property(s => s.MetaTitle).HasMaxLength(200);
        builder.Property(s => s.MetaDescription).HasMaxLength(500);
        builder.Property(s => s.IsActive).HasDefaultValue(true);

        builder.HasIndex(s => s.SortOrder);
        builder.HasIndex(s => s.Slug).IsUnique();

        builder.HasMany(s => s.Projects)
            .WithOne(p => p.Service)
            .HasForeignKey(p => p.ServiceId);

        builder.HasMany(s => s.Translations)
            .WithOne(t => t.Service)
            .HasForeignKey(t => t.ServiceId);
    }
}
