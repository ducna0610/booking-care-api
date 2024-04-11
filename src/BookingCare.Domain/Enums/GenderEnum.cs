namespace BookingCare.Domain.Enums
{
    public enum GenderEnum
    {
        [EnDescription("Female")]
        [ViDescription("Nữ")]
        FEMALE,
        [EnDescription("Male")]
        [ViDescription("Nam")]
        MALE,
        [EnDescription("None")]
        [ViDescription("Khác")]
        NONE
    }
}
