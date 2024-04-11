namespace BookingCare.Domain.Entities
{
    public class Language
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Translation> Translations { get; set; }
    }
}
