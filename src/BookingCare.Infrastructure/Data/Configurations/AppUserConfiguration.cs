using BookingCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingCare.Infrastructure.Data.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable("AppUsers");

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(30);
        builder.Property(x => x.UserName)
            .IsRequired()
            .HasMaxLength(30);
        builder.Property(x => x.NormalizedUserName)
            .IsRequired()
            .HasMaxLength(30);
        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(30);
        builder.Property(x => x.NormalizedEmail)
            .IsRequired()
            .HasMaxLength(30);
        builder.Property(x => x.Address)
            .IsRequired()
            .HasMaxLength(30);
        builder.Property(x => x.Gender)
            .HasColumnType("tinyint")
            .IsRequired();
        builder.Property(x => x.DateOfBirth)
            .IsRequired();
        builder.Property(x => x.RefreshToken)
            .HasMaxLength(200);
        builder.Property(x => x.PhoneNumber)
            .HasMaxLength(11);

        builder.HasIndex(x => x.PhoneNumber)
            .IsUnique();


        builder.HasOne(x => x.Ward)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.WardId)
            .IsRequired();

        builder.ToTable(x => x.HasTrigger("DELETE_AppUser"));
    }
}