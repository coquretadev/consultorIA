using AiConsulting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AiConsulting.Infrastructure.Data.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name).HasMaxLength(200).IsRequired();
        builder.Property(c => c.Company).HasMaxLength(200);
        builder.Property(c => c.Sector).HasMaxLength(100);
        builder.Property(c => c.Email).HasMaxLength(200).IsRequired();
        builder.Property(c => c.Phone).HasMaxLength(50);
        builder.Property(c => c.Notes).HasMaxLength(2000);
        builder.Property(c => c.IsArchived).HasDefaultValue(false);

        builder.HasMany(c => c.Projects)
            .WithOne(p => p.Client)
            .HasForeignKey(p => p.ClientId);

        builder.HasMany(c => c.Opportunities)
            .WithOne(o => o.Client)
            .HasForeignKey(o => o.ClientId);
    }
}
