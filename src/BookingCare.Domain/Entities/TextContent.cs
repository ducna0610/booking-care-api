using BookingCare.Domain.Base;

namespace BookingCare.Domain.Entities
{
    public class TextContent : BaseEntity
    {
        public string OriginalText { get; set; }

        public virtual ICollection<Translation> Translations { get; set; }

        public virtual Clinic ClinicDescription { get; set; }
        public virtual Speciality SpecialityName { get; set; }
        public virtual Speciality SpecialityDescription { get; set; }
        public virtual DoctorInfo DoctorInfoPosition { get; set; }
        public virtual DoctorInfo DoctorInfoDescription { get; set; }
    }
}
