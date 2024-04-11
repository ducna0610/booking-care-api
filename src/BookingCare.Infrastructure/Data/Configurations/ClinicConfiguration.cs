using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BookingCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingCare.Infrastructure.Data.Configurations
{
    public class ClinicConfiguration : IEntityTypeConfiguration<Clinic>
    {
        public void Configure(EntityTypeBuilder<Clinic> builder)
        {
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(x => x.Address)
                .IsRequired()
                .HasMaxLength(30);


            builder.HasOne(x => x.DescriptionTextContent)
                .WithOne(x => x.ClinicDescription)
                .HasForeignKey<Clinic>(x => x.DescriptionId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Ward)
                .WithMany(x => x.Clinics)
                .HasForeignKey(x => x.WardId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.ToTable(x => x.HasTrigger("DELETE_Clinic"));
        }
    }
}