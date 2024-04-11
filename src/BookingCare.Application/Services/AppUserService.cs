using AutoMapper;
using BookingCare.Application.DTOs.Requests.User;
using BookingCare.Application.DTOs.Responses;
using BookingCare.Application.DTOs.Responses.User;
using BookingCare.Application.Repositories;
using BookingCare.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;
using System.Transactions;

namespace BookingCare.Application.Services
{
    public interface IAppUserService
    {
        Task<PaginationResponse<UserDetailResponse>> ToPagination(PaginationUserRequest request);
        Task<UserDetailResponse> GetById(string id);
        Task<UserResponse> Create(CreateUserRequest request);
        Task<IdentityResult> Update(string id, UpdateUserRequest request);
        Task<IdentityResult> Delete(string id);
    }

    public class AppUserService : IAppUserService
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AuthService> _stringLocalizer;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAppUserRepository _appUserRepository;

        public AppUserService
            (
                IMapper mapper,
                UserManager<AppUser> userManager,
                IAppUserRepository appUserRepository
            )
        {
            _mapper = mapper;
            _userManager = userManager;
            _appUserRepository = appUserRepository;
        }

        public async Task<PaginationResponse<UserDetailResponse>> ToPagination(PaginationUserRequest request)
        {
            Expression<Func<AppUser, bool>> FilterByName()
            {
                return x =>
                    x.Name.Contains(request.Name);
            }

            Expression<Func<AppUser, bool>> FilterByWardId()
            {
                return x =>
                    request.WardId == null ? true : x.WardId == request.WardId;
            }

            var filters = new List<Expression<Func<AppUser, bool>>>();
            filters.Add(FilterByName());
            filters.Add(FilterByWardId());

            Expression<Func<AppUser, object>> ward = x => x.Ward;
            Expression<Func<AppUser, object>> district = x => x.Ward.District;
            Expression<Func<AppUser, object>> province = x => x.Ward.District.Province;

            var includes = new List<Expression<Func<AppUser, object>>>();
            includes.Add(ward);
            includes.Add(district);
            includes.Add(province);

            var orders = new List<Func<IQueryable<AppUser>, IOrderedQueryable<AppUser>>>();

            //if (request.Orders != null && request.Orders.Count() > 0)
            //{
            //    foreach (var item in request.Orders)
            //    {
            //        Func<IQueryable<AppUser>, IOrderedQueryable<AppUser>> order = null;

            //        var objOrder = item.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //        if (objOrder.Length == 2)
            //        {
            //            if (objOrder[1].Contains("asc"))
            //            {
            //                switch (objOrder[0])
            //                {
            //                    case "Name":
            //                        order = x => x.OrderBy(x => x.Name);
            //                        break;
            //                    case "CreatedAt":
            //                        order = x => x.OrderBy(x => x.CreatedAt);
            //                        break;
            //                }
            //            }
            //            else
            //            {
            //                switch (objOrder[0])
            //                {
            //                    case "Name":
            //                        order = x => x.OrderByDescending(x => x.Name);
            //                        break;
            //                    case "CreatedAt":
            //                        order = x => x.OrderByDescending(x => x.CreatedAt);
            //                        break;
            //                }
            //            }
            //        }

            //        if (order != null)
            //        {
            //            orders.Add(order);
            //        }
            //    }
            //}

            var users = await _appUserRepository.ToPaginationAsync(request.PageIndex, request.PageSize, filters, includes, orders);
            var result = _mapper.Map<PaginationResponse<UserDetailResponse>>(users);

            foreach (var item in result.Items)
            {
                var user = await _appUserRepository.GetByIdAsync(item.Id);
                item.Roles = await _userManager.GetRolesAsync(user);
            }

            return result;
        }

        public async Task<UserDetailResponse> GetById(string id)
        {
            Expression<Func<AppUser, bool>> FilterById()
            {
                return x => x.Id == Guid.Parse(id);
            }

            var filters = new List<Expression<Func<AppUser, bool>>>();
            filters.Add(FilterById());

            Expression<Func<AppUser, object>> ward = x => x.Ward;
            Expression<Func<AppUser, object>> district = x => x.Ward.District;
            Expression<Func<AppUser, object>> province = x => x.Ward.District.Province;
            var includes = new List<Expression<Func<AppUser, object>>>();
            includes.Add(ward);
            includes.Add(district);
            includes.Add(province);

            var user = (await _appUserRepository.GetAsync(filters, includes)).First();
            var data = _mapper.Map<UserDetailResponse>(user);
            data.Roles = await _userManager.GetRolesAsync(user);

            return data;
        }


        public async Task<UserResponse> Create(CreateUserRequest request)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var user = await _userManager.FindByEmailAsync(request.Email);

                    if (user != null)
                    {
                        throw new Exception(_stringLocalizer["UserIsExist"]);
                    }

                    user = _mapper.Map<AppUser>(request);
                    user.EmailConfirmed = true;

                    _userManager.CreateAsync(user, request.PhoneNumber);

                    await _userManager.AddToRolesAsync(user, request.Roles);

                    // Get the roles for the user
                    var data = _mapper.Map<UserResponse>(user);

                    data.Roles = await _userManager.GetRolesAsync(user);

                    scope.Complete();
                    return data;
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;
                }
            }
        }

        public async Task<IdentityResult> Update(string id, UpdateUserRequest request)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                throw new Exception(_stringLocalizer["UserIsNotExist"]);
            }

            // mapper
            user.Name = request.Name;
            user.Address = request.Address;
            user.PhoneNumber = request.PhoneNumber;
            user.DateOfBirth = (DateTimeOffset)request.DateOfBirth;
            user.Gender = request.Gender;
            user.WardId = request.WardId;

            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            result = await _userManager.AddToRolesAsync(user, request.Roles);

            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                throw new Exception(_stringLocalizer["UserIsNotExist"]);
            }

            return await _userManager.DeleteAsync(user);
        }
    }
}
