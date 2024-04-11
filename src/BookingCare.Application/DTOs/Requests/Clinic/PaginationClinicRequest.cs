namespace BookingCare.Application.DTOs.Requests.Clinic
{
    public class PaginationClinicRequest : PaginationRequest
    {
        public string Name { get; set; } = "";
        public int? WardId { get; set; }
    }
}
