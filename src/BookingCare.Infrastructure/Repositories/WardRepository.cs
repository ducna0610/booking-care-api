using BookingCare.Application.Repositories;
using BookingCare.Domain.Entities;
using BookingCare.Infrastructure.Data;

namespace BookingCare.Infrastructure.Repositories
{
    public class WardRepository : GenericRepository<Ward>, IWardRepository
    {
        public WardRepository(BookingCareDbContext context) : base(context)
        {
        }
    }
}
