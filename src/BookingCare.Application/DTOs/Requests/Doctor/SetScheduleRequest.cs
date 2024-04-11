using BookingCare.Domain.Enums;

namespace BookingCare.Application.DTOs.Requests.Doctor
{
    public class SetScheduleRequest
    {
        public Guid? DoctorId { get; set; }
        public DateTimeOffset? Date { get; set; }
        public List<TimeSelectEnum>? TimeSelects { get; set; }
    }
}
