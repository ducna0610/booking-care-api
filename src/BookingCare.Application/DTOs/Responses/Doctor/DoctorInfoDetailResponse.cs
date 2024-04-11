namespace BookingCare.Application.DTOs.Responses.Doctor
{
    public class DoctorInfoDetailResponse
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
        public string WardName { get; set; }
        public string DistrictName { get; set; }
        public string ProvinceName { get; set; }

        public string PositionId { get; set; }
        public string Position { get; set; }
        public string DescriptionId { get; set; }
        public string Description { get; set; }
        public int MaxPatient { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public string ClinicId { get; set; }
        public string ClinicName { get; set; }
        public string SpecialityId { get; set; }
        public string SpecialityName { get; set; }
    }
}