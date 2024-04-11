using System.ComponentModel;

namespace BookingCare.Application.DTOs.Requests
{

    public class PaginationRequest
    {
        [DefaultValue(0)]
        public int PageIndex { get; set; } = 0;

        [DefaultValue(10)]
        public int PageSize { get; set; } = 10;
    }
}
