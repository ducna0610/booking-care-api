using BookingCare.Application.DTOs.Responses;
using BookingCare.Domain.Entities;

namespace BookingCare.Application.Repositories
{
    public interface IClinicRepository : IGenericRepository<Clinic>
    {
        //IAddressDetailResponse GetAddressDetail(Guid id);
    }
}
