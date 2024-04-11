namespace BookingCare.Application.DTOs.Responses.Doctor
{
    public class ScheduleResponse
    {
        public Guid Id { get; set; }
        public Guid DoctorId { get; set; }
        public DateTimeOffset Date { get; set; }
        public KeyValuePair<int, string> TimeSelect { get; set; }
        public decimal Price { get; set; }
        public int CurrentPatient { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
