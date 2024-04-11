using Microsoft.AspNetCore.Mvc;

namespace BookingCare.Application.DTOs.Requests.Auth
{
    public class SignInRequest
    {
        public string? Email { get; set; }

        public string? Password { get; set; }

        public bool RememberMe { get; set; } = false;
    }
}
