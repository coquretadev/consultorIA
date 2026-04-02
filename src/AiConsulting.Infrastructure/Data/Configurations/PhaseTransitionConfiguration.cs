using AiConsulting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AiConsulting.Infrastructure.Data.Configurations;

public class PhaseTransitionConfiguration : IEntityTypeConfiguration<PhaseTransition>
{
    public void Configure(EntityTypeBuilder<PhaseTransition> builder)
    {
        builder.HasKey(pt => pt.Id);

        builder.Property(pt => pt.FromPhase).HasConversion<string>();
        builder.Property(pt => pt.ToPhase).HasConversion<string>();

        builder.HasOne(pt => pt.Opportunity)
            .WithMany(o => o.PhaseTransitions)
            .HasForeignKey(pt => pt.OpportunityId)
            .IsRequired();
    }
}
