using AiConsulting.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AiConsulting.Infrastructure.Data.Configurations;

public class ContactRequestConfiguration : IEntityTypeConfiguration<ContactRequest>
{
    public void Configure(EntityTypeBuilder<ContactRequest> builder)
    {
        builder.HasKey(cr => cr.Id);

        builder.Property(cr => cr.Name).HasMaxLength(200).IsRequired();
        builder.Property(cr => cr.Email).HasMaxLength(200).IsRequired();
        builder.Property(cr => cr.Company).HasMaxLength(200);
        builder.Property(cr => cr.Message).HasMaxLength(2000);
        builder.Property(cr => cr.IsProcessed).HasDefaultValue(false);
    }
}
