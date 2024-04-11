namespace BookingCare.Application.DTOs.Requests.User
{
    public class PaginationUserRequest : PaginationRequest
    {
        public string Name { get; set; } = "";
        public int? WardId { get; set; }
    }
}
