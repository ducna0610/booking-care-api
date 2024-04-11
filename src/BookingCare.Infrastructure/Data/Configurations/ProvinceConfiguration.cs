using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookingCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingCare.Infrastructure.Data.Configurations
{
    public class ProvinceConfiguration : IEntityTypeConfiguration<Province>
    {
        public void Configure(EntityTypeBuilder<Province> builder)
        {
            builder.Property(x => x.ProvinceName).IsRequired().HasMaxLength(30);

            builder.HasMany(x => x.Districts).WithOne(x => x.Province).HasForeignKey(x => x.ProvinceId);
        }
    }
}