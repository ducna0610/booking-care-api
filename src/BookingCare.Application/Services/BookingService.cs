using AutoMapper;
using BookingCare.Application.DTOs.Requests.Booking;
using BookingCare.Application.DTOs.Responses;
using BookingCare.Application.DTOs.Responses.Booking;
using BookingCare.Application.Repositories;
using BookingCare.Domain.Entities;
using BookingCare.Domain.Enums;
using System.Linq.Expressions;

namespace BookingCare.Application.Services
{
    public interface IBookingService
    {
        Task<PaginationResponse<BookingDetailResponse>> ToPagination(PaginationBookingRequest request);
        Task<BookingDetailResponse> GetById(Guid id);
        Task<BookingResponse> Create(CreateBookingRequest request);
        Task<BookingResponse> UpdateStatus(Guid id, UpdateStatusBookingRequest request);
    }

    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IBookingRepository _bookingRepository;

        public BookingService
            (
                IUnitOfWork unitOfWork,
                IMapper mapper,
                ICurrentUserService currentUserService,
                IBookingRepository bookingRepository
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _bookingRepository = bookingRepository;
        }

        public async Task<PaginationResponse<BookingDetailResponse>> ToPagination(PaginationBookingRequest request)
        {
            Expression<Func<Booking, object>> doctor = x => x.Doctor;
            Expression<Func<Booking, object>> schedule = x => x.Schedule;

            var includes = new List<Expression<Func<Booking, object>>>();
            includes.Add(doctor);
            includes.Add(schedule);

            var result = await _bookingRepository.ToPaginationAsync(request.PageIndex, request.PageSize, includes: includes);

            return _mapper.Map<PaginationResponse<BookingDetailResponse>>(result);
        }

        public async Task<BookingDetailResponse> GetById(Guid id)
        {
            Expression<Func<Booking, bool>> FilterById()
            {
                return x => x.Id == id;
            }

            var filters = new List<Expression<Func<Booking, bool>>>();
            filters.Add(FilterById());

            Expression<Func<Booking, object>> doctor = x => x.Doctor;
            Expression<Func<Booking, object>> schedule = x => x.Schedule;

            var includes = new List<Expression<Func<Booking, object>>>();
            includes.Add(doctor);
            includes.Add(schedule);

            var booking = await _bookingRepository.GetAsync(filters, includes);
            return _mapper.Map<BookingDetailResponse>(booking.First());
        }

        public async Task<BookingResponse> Create(CreateBookingRequest request)
        {
            var booking = _mapper.Map<Booking>(request);
            booking.PatientId = Guid.Parse(_currentUserService.UserId);
            booking.Status = StatusEnum.NEW;

            await _unitOfWork.ExecuteTransactionAsync(async () =>
            {
                await _bookingRepository.AddAsync(booking);
            });

            return _mapper.Map<BookingResponse>(booking);
        }

        public async Task<BookingResponse> UpdateStatus(Guid id, UpdateStatusBookingRequest request)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);

            booking.Status = request.Status;

            await _unitOfWork.ExecuteTransactionAsync(() =>
            {
                _bookingRepository.Update(booking);
            });

            return _mapper.Map<BookingResponse>(booking);
        }
    }
}
