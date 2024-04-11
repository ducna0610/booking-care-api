using BookingCare.Domain.Base;
using BookingCare.Domain.Enums;

namespace BookingCare.Domain.Entities
{
    public class Schedule : BaseEntity
    {
        public Guid DoctorId { get; set; }
        public DateTimeOffset Date { get; set; }
        public TimeSelectEnum TimeSelect { get; set; }
        public decimal Price { get; set; }
        public int CurrentPatient { get; set; } = 0;

        public virtual AppUser Doctor { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
