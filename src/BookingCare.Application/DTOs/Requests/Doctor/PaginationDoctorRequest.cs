namespace BookingCare.Application.DTOs.Requests.Doctor
{
    public class PaginationDoctorRequest : PaginationRequest
    {
        public string Name { get; set; } = "";
        public int? WardId { get; set; }
    }
}
