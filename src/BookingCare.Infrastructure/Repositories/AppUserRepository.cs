using BookingCare.Application.Repositories;
using BookingCare.Domain.Entities;
using BookingCare.Infrastructure.Data;

namespace BookingCare.Infrastructure.Repositories
{
    public class AppUserRepository : GenericRepository<AppUser>, IAppUserRepository
    {
        public AppUserRepository(BookingCareDbContext context) : base(context)
        {
        }
    }
}
