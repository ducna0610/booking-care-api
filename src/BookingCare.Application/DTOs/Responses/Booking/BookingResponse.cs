using BookingCare.Domain.Enums;

namespace BookingCare.Application.DTOs.Responses.Booking
{
    public class BookingResponse
    {
        public Guid Id { get; set; }
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }
        public Guid ScheduleId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
    }
}
