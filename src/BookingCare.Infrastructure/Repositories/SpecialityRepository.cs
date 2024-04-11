using BookingCare.Application.Repositories;
using BookingCare.Domain.Entities;
using BookingCare.Infrastructure.Data;
namespace BookingCare.Infrastructure.Repositories
{
    public class SpecialityRepository : GenericRepository<Speciality>, ISpecialityRepository
    {
        public SpecialityRepository(BookingCareDbContext context) : base(context)
        {
        }
    }
}
