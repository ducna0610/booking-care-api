namespace BookingCare.Application.DTOs.Requests.Account
{
    public class ChangePasswordRequest
    {
        public string? OldPassWord { get; set; }
        public string? NewPassWord { get; set; }
    }
}
