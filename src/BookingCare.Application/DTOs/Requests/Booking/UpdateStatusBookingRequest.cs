using BookingCare.Domain.Enums;

namespace BookingCare.Application.DTOs.Requests.Booking
{
    public class UpdateStatusBookingRequest
    {
        public StatusEnum Status { get; set; }
    }
}
