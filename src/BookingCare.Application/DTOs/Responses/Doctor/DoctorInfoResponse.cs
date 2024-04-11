namespace BookingCare.Application.DTOs.Responses.Doctor
{
    public class DoctorInfoResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public string Address { get; set; }
        public string WardId { get; set; }

        public Guid PositionId { get; set; }
        public Guid DescriptionId { get; set; }
        public int MaxPatient { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public Guid ClinicId { get; set; }
        public Guid SpecialityId { get; set; }
    }
}
