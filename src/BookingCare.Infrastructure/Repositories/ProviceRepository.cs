using BookingCare.Application.Repositories;
using BookingCare.Domain.Entities;
using BookingCare.Infrastructure.Data;

namespace BookingCare.Infrastructure.Repositories
{
    public class ProviceRepository : GenericRepository<Province>, IProviceRepository
    {
        public ProviceRepository(BookingCareDbContext context) : base(context)
        {
        }
    }
}
