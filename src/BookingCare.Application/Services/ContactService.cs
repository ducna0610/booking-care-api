using AutoMapper;
using BookingCare.Application.DTOs.Requests;
using BookingCare.Application.DTOs.Requests.Contact;
using BookingCare.Application.DTOs.Responses;
using BookingCare.Application.DTOs.Responses.Contact;
using BookingCare.Application.Repositories;
using BookingCare.Domain.Entities;
using System.Linq.Expressions;

namespace BookingCare.Application.Services
{
    public interface IContactService
    {
        Task<PaginationResponse<ContactResponse>> ToPagination(PaginationRequest request);
        Task<ContactResponse> GetById(Guid id);
        Task<ContactResponse> Create(CreateContactRequest request);
        //Task<ClinicResponse> Update(Guid id, UpdateClinicRequest request);
        Task Delete(Guid id);
    }

    public class ContactService : IContactService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IContactRepository _contactRepository;

        public ContactService
            (
                IUnitOfWork unitOfWork,
                IMapper mapper,
                IContactRepository contactRepository
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _contactRepository = contactRepository;
        }

        public async Task<PaginationResponse<ContactResponse>> ToPagination(PaginationRequest request)
        {
            var orders = new List<Func<IQueryable<Contact>, IOrderedQueryable<Contact>>>();
            Func<IQueryable<Contact>, IOrderedQueryable<Contact>> order = x => x.OrderByDescending(x => x.CreatedAt);
            if (order != null)
            {
                orders.Add(order);
            }

            var result = await _contactRepository.ToPaginationAsync(request.PageIndex, request.PageSize, orderBy: orders);

            return _mapper.Map<PaginationResponse<ContactResponse>>(result);
        }

        public async Task<ContactResponse> GetById(Guid id)
        {
            Expression<Func<Contact, bool>> FilterById()
            {
                return x => x.Id == id;
            }

            var filters = new List<Expression<Func<Contact, bool>>>();
            filters.Add(FilterById());


            var contact = await _contactRepository.GetAsync(filters);

            if (contact.Count() > 0)
            {
                return _mapper.Map<ContactResponse>(contact.First());
            }
            return new ContactResponse();
        }


        public async Task<ContactResponse> Create(CreateContactRequest request)
        {
            var contact = _mapper.Map<Contact>(request);

            await _unitOfWork.ExecuteTransactionAsync(async () =>
            {
                await _contactRepository.AddAsync(contact);
            });

            return _mapper.Map<ContactResponse>(contact);
        }

        public async Task Delete(Guid id)
        {
            var contact = await _contactRepository.GetByIdAsync(id);

            await _unitOfWork.ExecuteTransactionAsync(() =>
            {
                _contactRepository.Delete(contact);
            });
        }
    }
}
