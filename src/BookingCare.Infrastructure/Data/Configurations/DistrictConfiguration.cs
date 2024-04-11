using BookingCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingCare.Infrastructure.Data.Configurations
{
    public class DistrictConfiguration : IEntityTypeConfiguration<District>
    {
        public void Configure(EntityTypeBuilder<District> builder)
        {
            builder.Property(x => x.DistrictName)
                .IsRequired()
                .HasMaxLength(30);

            builder.HasMany(x => x.Wards)
                .WithOne(x => x.District)
                .HasForeignKey(x => x.DistrictId);
        }
    }
}