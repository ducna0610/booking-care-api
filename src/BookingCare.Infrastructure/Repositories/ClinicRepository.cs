using BookingCare.Application.Repositories;
using BookingCare.Domain.Entities;
using BookingCare.Infrastructure.Data;

namespace BookingCare.Infrastructure.Repositories;

public class ClinicRepository : GenericRepository<Clinic>, IClinicRepository
{
    public ClinicRepository(BookingCareDbContext context) : base(context)
    {
    }

    //public IAddressDetailResponse GetAddressDetail(Guid id)
    //{
    //    var address = from c in _context.Clinics
    //                 join w in _context.Wards on c.WardId equals w.Id
    //                 join d in _context.Districts on w.DistrictId equals d.Id
    //                 join p in _context.Provinces on d.ProvinceId equals p.Id
    //                 where c.Id == id
    //                 select new AddressDetailResponse
    //                 { 
    //                     WardName = w.WardName, 
    //                     DistrictName = d.DistrictName, 
    //                     ProviceName = p.ProvinceName 
    //                 };

    //    return address.FirstOrDefault();
    //}
}
