namespace BookingCare.Application.DTOs.Requests.Contact
{
    public class CreateContactRequest
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Message { get; set; }
    }
}
