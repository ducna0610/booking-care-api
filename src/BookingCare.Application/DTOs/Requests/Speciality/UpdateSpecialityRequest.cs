using Microsoft.AspNetCore.Http;

namespace BookingCare.Application.DTOs.Requests.Speciality
{
    public class UpdateSpecialityRequest
    {
        public string? ViName { get; set; }
        public string? EnName { get; set; }
        public string? ViDescription { get; set; }
        public string? EnDescription { get; set; }
        public IFormFile? NewImage { get; set; }
    }
}
