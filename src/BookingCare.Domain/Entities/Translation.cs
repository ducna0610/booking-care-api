using BookingCare.Domain.Base;

namespace BookingCare.Domain.Entities
{
    public class Translation : IAuditable
    {
        public Guid TextContextId { get; set; }
        public string LanguageId { get; set; }
        public string Content { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }

        public virtual Language Language { get; set; }
        public virtual TextContent TextContent { get; set; }
    }
}
