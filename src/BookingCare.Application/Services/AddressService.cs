using AutoMapper;
using BookingCare.Application.DTOs.Responses.Address;
using BookingCare.Application.Repositories;
using BookingCare.Domain.Entities;
using System.Linq.Expressions;

namespace BookingCare.Application.Services
{
    public interface IAddressService
    {
        Task<IEnumerable<ProvinceResponse>> GetProvinces();
        Task<IEnumerable<DistrictResponse>> GetDistrictsByProvinceId(int provinceId);
        Task<IEnumerable<WardResponse>> GetWardsByDistrictId(int districtId);
    }

    public class AddressService : IAddressService
    {
        private readonly IMapper _mapper;
        private readonly IProviceRepository _proviceRepository;
        private readonly IDistrictRepository _districtRepository;
        private readonly IWardRepository _wardRepository;

        public AddressService(IMapper mapper, IProviceRepository proviceRepository, IDistrictRepository districtRepository,IWardRepository wardRepository)
        {
            _mapper = mapper;
            _proviceRepository = proviceRepository;
            _districtRepository = districtRepository;
            _wardRepository = wardRepository;
        }

        public async Task<IEnumerable<ProvinceResponse>> GetProvinces()
        {
            var provinces = await _proviceRepository.GetAsync();
            return _mapper.Map<List<ProvinceResponse>>(provinces);
        }

        public async Task<IEnumerable<DistrictResponse>> GetDistrictsByProvinceId(int provinceId)
        {
            Expression<Func<District, bool>> FilterByProvinceId()
            {
                return x =>
                    x.ProvinceId.Equals(provinceId);
            }

            var filters = new Expression<Func<District, bool>>[] { FilterByProvinceId() };

            var districts = await _districtRepository.GetAsync(filters);

            return _mapper.Map<List<DistrictResponse>>(districts);
        }

        public async Task<IEnumerable<WardResponse>> GetWardsByDistrictId(int districtId)
        {
            Expression<Func<Ward, bool>> FilterByDistrictId()
            {
                return x =>
                    x.DistrictId.Equals(districtId);
            }

            var filters = new Expression<Func<Ward, bool>>[] { FilterByDistrictId() };

            var wards = await _wardRepository.GetAsync(filters);

            return _mapper.Map<List<WardResponse>>(wards);
        }

    }
}
