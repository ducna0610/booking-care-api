namespace BookingCare.Application.DTOs.Responses.Payment
{
    public class PaymentReturnResponse
    {
        public bool Success { get; set; }
        public string OrderId { get; set; }
        public string OrderInfo { get; set; }
        public int Amount { get; set; }
    }
}
