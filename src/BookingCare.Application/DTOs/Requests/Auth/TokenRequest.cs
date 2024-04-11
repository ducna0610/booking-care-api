namespace BookingCare.Application.DTOs.Requests.Auth
{
    public class TokenRequest
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}