using BookingCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingCare.Infrastructure.Data.Configurations;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(30);
        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(30);
        builder.Property(x => x.Message)
            .IsRequired();
    }
}

