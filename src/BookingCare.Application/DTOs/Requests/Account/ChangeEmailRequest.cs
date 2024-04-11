namespace BookingCare.Application.DTOs.Requests.Account
{
    public class ChangeEmailRequest
    {
        public string? UserId { get; set; }

        public string? NewEmail { get; set; }
        public string? Code { get; set; }
    }
}
