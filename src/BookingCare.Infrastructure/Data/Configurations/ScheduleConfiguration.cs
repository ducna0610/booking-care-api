using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookingCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingCare.Infrastructure.Data.Configurations
{
    public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            builder.HasAlternateKey(x => new { x.DoctorId, x.Date, x.TimeSelect });

            builder.Property(x => x.Price).HasColumnType("money");

            builder.Property(x => x.Date)
                .IsRequired();
            builder.Property(x => x.TimeSelect)
                .HasColumnType("tinyint")
                .IsRequired();
        }
    }
}