using BookingCare.Application.Repositories;
using BookingCare.Domain.Entities;
using BookingCare.Infrastructure.Data;

namespace BookingCare.Infrastructure.Repositories;

public class ContactRepository : GenericRepository<Contact>, IContactRepository
{
    public ContactRepository(BookingCareDbContext context) : base(context)
    {
    }
}
