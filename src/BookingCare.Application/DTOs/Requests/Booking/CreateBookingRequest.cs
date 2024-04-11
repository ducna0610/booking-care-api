using BookingCare.Domain.Enums;

namespace BookingCare.Application.DTOs.Requests.Booking
{
    public class CreateBookingRequest
    {
        public Guid? DoctorId { get; set; }
        public Guid? ScheduleId { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public GenderEnum? Gender { get; set; }
        public DateTimeOffset? DateOfBirth { get; set; }
        public string? Note { get; set; }
    }
}
