using BookingCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingCare.Infrastructure.Data.Configurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(30);
            builder.Property(x => x.DateOfBirth)
                .IsRequired();
            builder.Property(x => x.Gender)
                .HasColumnType("tinyint")
                .IsRequired();
            builder.Property(x => x.Status)
                .HasColumnType("tinyint")
                .IsRequired();
            builder.Property(x => x.Note)
                .HasMaxLength(50);
            builder.Property(x => x.PhoneNumber)
                .HasMaxLength(11);

            builder.HasIndex(x => x.PhoneNumber)
                .IsUnique();


            builder.HasOne(x => x.Doctor)
                .WithMany(x => x.DotorBookings)
                .HasForeignKey(x => x.DoctorId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Patient)
                .WithMany(x => x.PatientBookings)
                .HasForeignKey(x => x.PatientId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Schedule)
                .WithMany(x => x.Bookings)
                .HasForeignKey(x => x.ScheduleId);
        }
    }
}