namespace BookingCare.Application.DTOs.Requests.Booking
{
    public class PaginationBookingRequest : PaginationRequest
    {
        public string? DoctorName { get; set; }
    }
}
