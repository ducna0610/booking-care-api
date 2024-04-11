namespace BookingCare.Domain.Entities
{
    public class Ward
    {
        public int Id { get; set; }
        public string WardName { get; set; }
        public int DistrictId { get; set; }

        public virtual District District { get; set; }
        public virtual ICollection<Clinic> Clinics { get; set; }
        public virtual ICollection<AppUser> Users { get; set; }
    }
}
