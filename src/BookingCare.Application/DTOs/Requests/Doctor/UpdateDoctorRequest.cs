using BookingCare.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace BookingCare.Application.DTOs.Requests.Doctor
{
    public class UpdateDoctorRequest
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTimeOffset? DateOfBirth { get; set; }
        public GenderEnum? Gender { get; set; }
        public int? WardId { get; set; }

        public string? ViPosition { get; set; }
        public string? EnPosition { get; set; }
        public string? ViDescription { get; set; }
        public string? EnDescription { get; set; }
        public Guid? ClinicId { get; set; }
        public Guid? SpecialityId { get; set; }
        public int MaxPatient { get; set; } = 5;
        public decimal? Price { get; set; }
        public IFormFile? NewImage { get; set; }
    }
}
