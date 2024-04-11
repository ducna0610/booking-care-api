using BookingCare.Domain.Base;

namespace BookingCare.Domain.Entities
{
    public class Speciality : BaseEntity
    {
        public Guid NameId { get; set; }
        public Guid DescriptionId { get; set; }
        public string? Image { get; set; }

        public virtual TextContent NameTextContent { get; set; }
        public virtual TextContent DescriptionTextContent { get; set; }
        public virtual ICollection<DoctorInfo> DoctorInfos { get; set; }
    }
}
