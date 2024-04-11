namespace BookingCare.Domain.Enums
{
    public enum StatusEnum
    {
        [EnDescription("New")]
        [ViDescription("Mới tạo")]
        NEW,
        [EnDescription("Confirmed")]
        [ViDescription("Xác nhận")]
        CONFIRMED,
        [EnDescription("Done")]
        [ViDescription("Đã khám")]
        DONE,
        [EnDescription("Completed")]
        [ViDescription("Đã thanh toán")]
        COMPLETED,
        [EnDescription("Cancel")]
        [ViDescription("Đã hủy")]
        CANCEL
    }
}
