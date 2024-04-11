using BookingCare.Application.Repositories;
using BookingCare.Domain.Entities;
using BookingCare.Infrastructure.Data;

namespace BookingCare.Infrastructure.Repositories
{
    public class TranslationRepository : GenericRepository<Translation>, ITranslationRepository
    {
        public TranslationRepository(BookingCareDbContext context) : base(context) { }
    }
}
