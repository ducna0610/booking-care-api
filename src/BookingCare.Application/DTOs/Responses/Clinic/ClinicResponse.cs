namespace BookingCare.Application.DTOs.Responses.Clinic
{
    public class ClinicResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid DescriptionId { get; set; }
        public string Image { get; set; }
        public int WardId { get; set; }
        //public DateTimeOffset CreatedAt { get; set; }
    }
}
