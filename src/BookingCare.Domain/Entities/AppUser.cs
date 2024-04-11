using BookingCare.Domain.Base;
using BookingCare.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace BookingCare.Domain.Entities;

public class AppUser : IdentityUser<Guid>, IAuditable
{
    public string Name { get; set; }
    public GenderEnum? Gender { get; set; }
    public string? RefreshToken { get; set; }
    public DateTimeOffset DateOfBirth { get; set; }
    public DateTimeOffset RefreshTokenExpiryTime { get; set; }
    public string? Address { get; set; }
    public int? WardId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    public virtual ICollection<Booking>? DotorBookings { get; set; }
    public virtual ICollection<Booking>? PatientBookings { get; set; }
    public virtual DoctorInfo? DoctorInfo { get; set; }
    public virtual ICollection<Schedule>? Schedules { get; set; }
    public virtual Ward? Ward { get; set; }
}
