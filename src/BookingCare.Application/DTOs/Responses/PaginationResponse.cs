namespace BookingCare.Application.DTOs.Responses
{
    public class PaginationResponse<T>
    {
        public int TotalRecords { get; set; }
        public int PageSize { get; set; }
        public int PagesCount
        {
            get
            {
                var pageCount = (double)TotalRecords / PageSize;
                return (int)Math.Ceiling(pageCount);
            }
        }
        public int PageIndex { get; set; }

        /// <summary>
        /// page number start from 0
        /// </summary>
        public bool Next => PageIndex + 1 < PagesCount;
        public bool Previous => PageIndex > 0;
        public ICollection<T> Items { get; set; } = new List<T>();
    }
}