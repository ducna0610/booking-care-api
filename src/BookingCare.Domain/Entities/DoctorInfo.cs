using BookingCare.Domain.Base;

namespace BookingCare.Domain.Entities
{
    public class DoctorInfo : BaseEntity
    {
        public string? Image { get; set; }
        public int MaxPatient { get; set; }
        public decimal Price { get; set; }
        public Guid ClinicId { get; set; }
        public Guid SpecialityId { get; set; }
        public Guid CurrencyId { get; set; }
        public Guid PositionId { get; set; }
        public Guid DescriptionId { get; set; }

        public virtual AppUser Doctor { get; set; }
        public virtual Clinic Clinic { get; set; }
        public virtual Speciality Speciality { get; set; }
        public virtual TextContent PositionTextContent { get; set; }
        public virtual TextContent DescriptionTextContent { get; set; }
    }
}
