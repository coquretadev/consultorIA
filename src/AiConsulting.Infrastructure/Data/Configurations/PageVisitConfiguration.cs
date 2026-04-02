using AiConsulting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AiConsulting.Infrastructure.Data.Configurations;

public class PageVisitConfiguration : IEntityTypeConfiguration<PageVisit>
{
    public void Configure(EntityTypeBuilder<PageVisit> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Page).HasMaxLength(500).IsRequired();
        builder.Property(p => p.Referrer).HasMaxLength(500);
        builder.Property(p => p.UserAgent).HasMaxLength(500);
        builder.Property(p => p.DeviceType).HasMaxLength(50).IsRequired();
        builder.Property(p => p.IpHash).HasMaxLength(64).IsRequired();

        builder.HasIndex(p => p.VisitedAt);
    }
}
