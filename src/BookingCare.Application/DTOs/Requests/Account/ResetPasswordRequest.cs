namespace BookingCare.Application.DTOs.Requests.Account
{
    public class ResetPasswordRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? Code { get; set; }
    }
}