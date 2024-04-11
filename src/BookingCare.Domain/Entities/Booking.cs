using BookingCare.Domain.Base;
using BookingCare.Domain.Enums;

namespace BookingCare.Domain.Entities;

public class Booking : BaseEntity
{
    public Guid DoctorId { get; set; }
    public Guid PatientId { get; set; }
    public StatusEnum Status { get; set; }
    public Guid ScheduleId { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public GenderEnum? Gender { get; set; }
    public DateTimeOffset DateOfBirth { get; set; }
    public string? Note { get; set; }

    public virtual AppUser Doctor { get; set; }
    public virtual AppUser Patient { get; set; }
    public virtual Schedule Schedule { get; set; }
}
