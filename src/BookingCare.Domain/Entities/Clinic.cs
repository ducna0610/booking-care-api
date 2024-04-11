using BookingCare.Domain.Base;

namespace BookingCare.Domain.Entities;

public class Clinic : BaseEntity
{
    public string Name { get; set; }
    public string Address { get; set; }
    public Guid DescriptionId { get; set; }
    public string? Image { get; set; }
    public int WardId { get; set; }

    public virtual Ward Ward { get; set; }
    public virtual TextContent DescriptionTextContent { get; set; }
    public virtual ICollection<DoctorInfo> DoctorInfos { get; set; }
}
