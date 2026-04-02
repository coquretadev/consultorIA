using AiConsulting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AiConsulting.Infrastructure.Data.Configurations;

public class ConsultorAvailabilityConfiguration : IEntityTypeConfiguration<ConsultorAvailability>
{
    public void Configure(EntityTypeBuilder<ConsultorAvailability> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.DayOfWeek).IsRequired();
        builder.Property(c => c.StartTime).IsRequired();
        builder.Property(c => c.EndTime).IsRequired();
        builder.Property(c => c.IsActive).HasDefaultValue(true);
    }
}
