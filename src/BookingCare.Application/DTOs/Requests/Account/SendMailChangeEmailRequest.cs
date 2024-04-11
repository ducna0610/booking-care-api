namespace BookingCare.Application.DTOs.Requests.Account
{
    public class SendMailChangeEmailRequest
    {
        public string? UserId { get; set; }
        public string? NewEmail { get; set; }
    }
}
