using AiConsulting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AiConsulting.Infrastructure.Data.Configurations;

public class OpportunityConfiguration : IEntityTypeConfiguration<Opportunity>
{
    public void Configure(EntityTypeBuilder<Opportunity> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.ContactName).HasMaxLength(200).IsRequired();
        builder.Property(o => o.ContactEmail).HasMaxLength(200).IsRequired();
        builder.Property(o => o.Company).HasMaxLength(200);
        builder.Property(o => o.Message).HasMaxLength(2000);
        builder.Property(o => o.EstimatedValue).HasPrecision(18, 2);
        builder.Property(o => o.CurrentPhase).HasConversion<string>();

        builder.HasOne(o => o.Client)
            .WithMany(c => c.Opportunities)
            .HasForeignKey(o => o.ClientId)
            .IsRequired(false);

        builder.HasMany(o => o.PhaseTransitions)
            .WithOne(pt => pt.Opportunity)
            .HasForeignKey(pt => pt.OpportunityId);
    }
}
