namespace BookingCare.Domain.Entities
{
    public class District
    {
        public int Id { get; set; }
        public string DistrictName { get; set; }
        public int ProvinceId { get; set; }

        public virtual Province Province { get; set; }
        public virtual ICollection<Ward> Wards { get; set; }
    }
}
