namespace BookingCare.Domain.Entities
{
    public class Province
    {
        public int Id { get; set; }
        public string ProvinceName { get; set; }

        public virtual ICollection<District> Districts { get; set; }
    }
}
