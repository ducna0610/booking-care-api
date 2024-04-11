using BookingCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingCare.Infrastructure.Data.Configurations
{
    public class DoctorInfoConfiguration : IEntityTypeConfiguration<DoctorInfo>
    {
        public void Configure(EntityTypeBuilder<DoctorInfo> builder)
        {
            builder.Property(x => x.Id).HasColumnName("DoctorId");

            builder.Property(x => x.Price).HasColumnType("money");

            builder.HasOne(x => x.Doctor)
                .WithOne(x => x.DoctorInfo)
                .HasForeignKey<DoctorInfo>(x => x.Id);

            builder.HasOne(x => x.Clinic)
                .WithMany(x => x.DoctorInfos)
                .HasForeignKey(x => x.ClinicId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Speciality)
                .WithMany(x => x.DoctorInfos)
                .HasForeignKey(x => x.SpecialityId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.PositionTextContent)
                .WithOne(x => x.DoctorInfoPosition)
                .HasForeignKey<DoctorInfo>(x => x.PositionId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.DescriptionTextContent)
                .WithOne(x => x.DoctorInfoDescription)
                .HasForeignKey<DoctorInfo>(x => x.DescriptionId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.ToTable(x => x.HasTrigger("DELETE_DoctorInfo"));
        }
    }
}