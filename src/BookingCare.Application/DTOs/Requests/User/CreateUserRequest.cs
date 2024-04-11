using BookingCare.Domain.Enums;

namespace BookingCare.Application.DTOs.Requests.User
{
    public class CreateUserRequest
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTimeOffset? DateOfBirth { get; set; }
        public GenderEnum? Gender { get; set; }
        public int? WardId { get; set; }
        public IList<string>? Roles { get; set; }
    }
}
