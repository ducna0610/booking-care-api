namespace BookingCare.Application.DTOs.Requests.Doctor
{
    public class GetScheduleRequest
    {
        public Guid? DoctorId { get; set; }
        public DateTimeOffset? Date { get; set; }
    }
}
