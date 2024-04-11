using BookingCare.Application.Repositories;
using BookingCare.Domain.Entities;
using BookingCare.Infrastructure.Data;

namespace BookingCare.Infrastructure.Repositories
{
    public class TextContextRepository : GenericRepository<TextContent>, ITextContextRepository
    {
        public TextContextRepository(BookingCareDbContext context) : base(context) { }
    }
}
