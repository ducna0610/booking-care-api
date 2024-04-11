namespace BookingCare.Application.DTOs.Responses.Contact
{
    public class ContactResponse
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
