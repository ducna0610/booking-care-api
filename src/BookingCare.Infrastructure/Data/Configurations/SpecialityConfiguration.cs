using BookingCare.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace BookingCare.Infrastructure.Data.Configurations
{
    public class SpecialityConfiguration : IEntityTypeConfiguration<Speciality>
    {
        public void Configure(EntityTypeBuilder<Speciality> builder)
        {
            builder.HasOne(x => x.NameTextContent)
                .WithOne(x => x.SpecialityDescription)
                .HasForeignKey<Speciality>(x => x.NameId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.DescriptionTextContent)
                .WithOne(x => x.SpecialityName)
                .HasForeignKey<Speciality>(x => x.DescriptionId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.ToTable(x => x.HasTrigger("DELETE_Speciality"));
        }
    }
}