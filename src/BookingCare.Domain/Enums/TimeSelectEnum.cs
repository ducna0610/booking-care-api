namespace BookingCare.Domain.Enums
{
    public enum TimeSelectEnum
    {
        [EnDescription("8:00 AM - 9:00 AM")]
        [ViDescription("8:00 giờ - 9:00 giờ")]
        T1 = 1,
        [ViDescription("9:00 giờ - 10:00 giờ")]
        [EnDescription("9:00 AM - 10:00 AM")]
        T2,
        [ViDescription("10:00 giờ - 11:00 giờ")]
        [EnDescription("10:00 AM- 11:00 AM")]
        T3,
        [ViDescription("11:00 giờ - 12:00 giờ")]
        [EnDescription("11:00 AM - 12:00 AM")]
        T4,

        [ViDescription("13:00 giờ - 14:00 giờ")]
        [EnDescription("13:00 PM - 14:00 PM")]
        T5,
        [ViDescription("14:00 giờ - 15:00 giờ")]
        [EnDescription("14:00 PM - 15:00 PM")]
        T6,
        [ViDescription("15:00 giờ - 16:00 giờ")]
        [EnDescription("15:00 PM - 16:00 PM")]
        T7,
        [ViDescription("16:00 giờ - 17:00 giờ")]
        [EnDescription("16:00 PM - 17:00 PM")]
        T8,
    }
}
