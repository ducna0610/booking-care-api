using BookingCare.Domain.Base;

namespace BookingCare.Domain.Entities;

public class Contact : BaseEntity
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Message { get; set; }
}
