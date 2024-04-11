using BookingCare.Application.Repositories;
using BookingCare.Domain.Entities;
using BookingCare.Infrastructure.Data;

namespace BookingCare.Infrastructure.Repositories
{
    public class DoctorInfoRepository : GenericRepository<DoctorInfo>, IDoctorInfoRepository
    {
        public DoctorInfoRepository(BookingCareDbContext context) : base(context)
        {
        }
    }
}
