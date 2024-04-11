using AutoMapper;
using BookingCare.Application.DTOs.Requests.Doctor;
using BookingCare.Application.DTOs.Responses;
using BookingCare.Application.DTOs.Responses.Doctor;
using BookingCare.Application.Repositories;
using BookingCare.Application.Utils;
using BookingCare.Domain.Base;
using BookingCare.Domain.Common;
using BookingCare.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;

namespace BookingCare.Application.Services
{
    public interface IDoctorService
    {
        Task<PaginationResponse<DoctorInfoDetailResponse>> ToPagination(PaginationDoctorRequest request);
        Task<DoctorInfoDetailResponse> GetById(Guid id);
        Task<DoctorInfoResponse> Create(CreateDoctorRequest request);
        Task<DoctorInfoResponse> Update(Guid id, UpdateDoctorRequest request);
        Task<List<ScheduleResponse>> GetSchedule(GetScheduleRequest request);
        Task<List<ScheduleResponse>> SetSchedule(SetScheduleRequest request);
    }

    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AuthService> _stringLocalizer;
        private readonly IFileStorageService _fileStorageService;
        private readonly ITextContextRepository _textContextRepository;
        private readonly ITranslationRepository _translationRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAppUserRepository _appUserRepository;
        private readonly IDoctorInfoRepository _doctorInfoRepository;
        private readonly IScheduleRepository _scheduleRepository;

        public DoctorService
            (
                IUnitOfWork unitOfWork,
                IMapper mapper,
                IStringLocalizer<AuthService> stringLocalizer,
                IFileStorageService fileStorageService,
                ITextContextRepository textContextRepository,
                ITranslationRepository translationRepository,
                UserManager<AppUser> userManager,
                IAppUserRepository appUserRepository,
                IDoctorInfoRepository doctorInfoRepository,
                IScheduleRepository scheduleRepository
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
            _fileStorageService = fileStorageService;
            _textContextRepository = textContextRepository;
            _translationRepository = translationRepository;
            _userManager = userManager;
            _appUserRepository = appUserRepository;
            _doctorInfoRepository = doctorInfoRepository;
            _scheduleRepository = scheduleRepository;
        }

        public async Task<PaginationResponse<DoctorInfoDetailResponse>> ToPagination(PaginationDoctorRequest request)
        {
            var doctorIds = (await _userManager.GetUsersInRoleAsync(RoleName.Doctor)).Select(x => x.Id);

            Expression<Func<AppUser, bool>> FilterByRoles()
            {
                return x => doctorIds.Contains(x.Id);
            }

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
            filters.Add(FilterByRoles());
            filters.Add(FilterByName());
            filters.Add(FilterByWardId());

            Expression<Func<AppUser, object>> ward = x => x.Ward;
            Expression<Func<AppUser, object>> district = x => x.Ward.District;
            Expression<Func<AppUser, object>> province = x => x.Ward.District.Province;
            Expression<Func<AppUser, object>> doctorInfo = x => x.DoctorInfo;
            Expression<Func<AppUser, object>> position = x => x.DoctorInfo.PositionTextContent.Translations;
            Expression<Func<AppUser, object>> description = x => x.DoctorInfo.DescriptionTextContent.Translations;
            Expression<Func<AppUser, object>> clinic = x => x.DoctorInfo.Clinic;
            Expression<Func<AppUser, object>> speciality = x => x.DoctorInfo.Speciality;
            Expression<Func<AppUser, object>> specialityName = x => x.DoctorInfo.Speciality.NameTextContent.Translations;
            var includes = new List<Expression<Func<AppUser, object>>>();
            includes.Add(ward);
            includes.Add(district);
            includes.Add(province);
            includes.Add(doctorInfo);
            includes.Add(position);
            includes.Add(description);
            includes.Add(clinic);
            includes.Add(speciality);
            includes.Add(specialityName);

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
            var result = _mapper.Map<PaginationResponse<DoctorInfoDetailResponse>>(users);

            foreach (var item in result.Items)
            {
                var user = await _appUserRepository.GetByIdAsync(item.Id);
                item.Roles = await _userManager.GetRolesAsync(user);
            }

            return result;
        }

        public async Task<DoctorInfoDetailResponse> GetById(Guid id)
        {
            Expression<Func<AppUser, bool>> FilterById()
            {
                return x => x.Id == id;
            }

            var filters = new List<Expression<Func<AppUser, bool>>>();
            filters.Add(FilterById());

            Expression<Func<AppUser, object>> ward = x => x.Ward;
            Expression<Func<AppUser, object>> district = x => x.Ward.District;
            Expression<Func<AppUser, object>> province = x => x.Ward.District.Province;
            Expression<Func<AppUser, object>> doctorInfo = x => x.DoctorInfo;
            Expression<Func<AppUser, object>> position = x => x.DoctorInfo.PositionTextContent.Translations;
            Expression<Func<AppUser, object>> description = x => x.DoctorInfo.DescriptionTextContent.Translations;
            Expression<Func<AppUser, object>> clinic = x => x.DoctorInfo.Clinic;
            Expression<Func<AppUser, object>> speciality = x => x.DoctorInfo.Speciality;
            Expression<Func<AppUser, object>> specialityName = x => x.DoctorInfo.Speciality.NameTextContent.Translations;
            var includes = new List<Expression<Func<AppUser, object>>>();
            includes.Add(ward);
            includes.Add(district);
            includes.Add(province);
            includes.Add(doctorInfo);
            includes.Add(position);
            includes.Add(description);
            includes.Add(clinic);
            includes.Add(speciality);
            includes.Add(specialityName);

            var user = (await _appUserRepository.GetAsync(filters, includes)).First();
            var data = _mapper.Map<DoctorInfoDetailResponse>(user);
            data.Roles = await _userManager.GetRolesAsync(user);

            return data;
        }

        public async Task<DoctorInfoResponse> Create(CreateDoctorRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user != null)
            {
                throw new Exception(_stringLocalizer["UserIsExist"]);
            }

            user = _mapper.Map<AppUser>(request);
            user.EmailConfirmed = true;

            // doctor info
            var positionTranslations = new List<Translation>();
            var positionTextContext = new TextContent()
            {
                OriginalText = request.EnPosition,
            };
            positionTranslations.Add(
                new Translation()
                {
                    TextContextId = positionTextContext.Id,
                    LanguageId = LanguageCode.English,
                    Content = request.EnPosition
                }
            );
            positionTranslations.Add(
                new Translation()
                {
                    TextContextId = positionTextContext.Id,
                    LanguageId = LanguageCode.VietNamese,
                    Content = request.ViPosition
                }
            );

            var descriptionTranslations = new List<Translation>();
            var descriptionTextContext = new TextContent()
            {
                OriginalText = request.EnDescription,
            };
            descriptionTranslations.Add(
                new Translation()
                {
                    TextContextId = descriptionTextContext.Id,
                    LanguageId = LanguageCode.English,
                    Content = request.EnDescription
                }
            );
            descriptionTranslations.Add(
                new Translation()
                {
                    TextContextId = descriptionTextContext.Id,
                    LanguageId = LanguageCode.VietNamese,
                    Content = request.ViDescription
                }
            );

            var doctorInfo = _mapper.Map<DoctorInfo>(request);
            doctorInfo.PositionId = positionTextContext.Id;
            doctorInfo.DescriptionId = descriptionTextContext.Id;
            doctorInfo.MaxPatient = request.MaxPatient;
            doctorInfo.Image = await _fileStorageService.UploadAsync(request.Image);

            await _unitOfWork.ExecuteTransactionAsync(async () =>
            {
                var result = await _userManager.CreateAsync(user, "123456a@");
                await _userManager.AddToRoleAsync(user, RoleName.Doctor);
                await _textContextRepository.AddAsync(positionTextContext);
                await _translationRepository.AddRangeAsync(positionTranslations);
                await _textContextRepository.AddAsync(descriptionTextContext);
                await _translationRepository.AddRangeAsync(descriptionTranslations);
                doctorInfo.Id = user.Id;
                await _doctorInfoRepository.AddAsync(doctorInfo);
            });

            // Get the roles for the user
            var data = _mapper.Map<DoctorInfoResponse>(user);
            data.Roles = await _userManager.GetRolesAsync(user);

            return data;
        }

        public async Task<DoctorInfoResponse> Update(Guid id, UpdateDoctorRequest request)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

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

            var oldDoctorInfo = user.DoctorInfo;
            var doctorInfo = _mapper.Map<DoctorInfo>(user);
            doctorInfo.Id = oldDoctorInfo.Id;
            doctorInfo.PositionId = oldDoctorInfo.PositionId;
            doctorInfo.DescriptionId = oldDoctorInfo.DescriptionId;
            doctorInfo.ClinicId = oldDoctorInfo.ClinicId;
            doctorInfo.SpecialityId = oldDoctorInfo.SpecialityId;
            doctorInfo.Image = oldDoctorInfo.Image;
            doctorInfo.CreatedAt = oldDoctorInfo.CreatedAt;


            if (request.NewImage != null)
            {
                await _fileStorageService.DeleteAsync(doctorInfo.Image);
                doctorInfo.Image = await _fileStorageService.UploadAsync(request.NewImage);
            }

            Expression<Func<Translation, bool>> enFilterByPositionId()
            {
                return x => x.TextContextId == oldDoctorInfo.PositionId && x.LanguageId == LanguageCode.English;
            }
            var enFilterByPositionIdTranslations = new List<Expression<Func<Translation, bool>>>();
            enFilterByPositionIdTranslations.Add(enFilterByDescriptionId());
            var enPositionTranslation = (await _translationRepository.GetAsync(enFilterByPositionIdTranslations)).First();
            enPositionTranslation.Content = request.EnPosition;


            Expression<Func<Translation, bool>> viFilterByPositionId()
            {
                return x => x.TextContextId == oldDoctorInfo.PositionId && x.LanguageId == LanguageCode.VietNamese;
            }
            var viFilterByPositionIdTranslations = new List<Expression<Func<Translation, bool>>>();
            viFilterByPositionIdTranslations.Add(viFilterByPositionId());
            var viPositionTranslation = (await _translationRepository.GetAsync(viFilterByPositionIdTranslations)).First();
            viPositionTranslation.Content = request.ViPosition;

            Expression<Func<Translation, bool>> enFilterByDescriptionId()
            {
                return x => x.TextContextId == oldDoctorInfo.DescriptionId && x.LanguageId == LanguageCode.English;
            }
            var enFilterByDescriptionIdTranslations = new List<Expression<Func<Translation, bool>>>();
            enFilterByDescriptionIdTranslations.Add(enFilterByDescriptionId());
            var enDescriptionTranslation = (await _translationRepository.GetAsync(enFilterByDescriptionIdTranslations)).First();
            enDescriptionTranslation.Content = request.EnDescription;


            Expression<Func<Translation, bool>> viFilterByDescriptionId()
            {
                return x => x.TextContextId == oldDoctorInfo.DescriptionId && x.LanguageId == LanguageCode.VietNamese;
            }
            var viFilterByDescriptionIdTranslations = new List<Expression<Func<Translation, bool>>>();
            viFilterByDescriptionIdTranslations.Add(enFilterByDescriptionId());
            var viDescriptionTranslation = (await _translationRepository.GetAsync(viFilterByDescriptionIdTranslations)).First();
            viDescriptionTranslation.Content = request.ViDescription;


            await _unitOfWork.ExecuteTransactionAsync(async () =>
            {
                await _userManager.UpdateAsync(user);
                _doctorInfoRepository.Update(doctorInfo);
                _translationRepository.Update(enPositionTranslation);
                _translationRepository.Update(viPositionTranslation);
                _translationRepository.Update(viDescriptionTranslation);
                _translationRepository.Update(viDescriptionTranslation);
            });

            return _mapper.Map<DoctorInfoResponse>(user);
        }
        public async Task<List<ScheduleResponse>> GetSchedule(GetScheduleRequest request)
        {
            Expression<Func<Schedule, bool>> FilterSchedule()
            {
                return x => x.DoctorId == request.DoctorId && x.Date == request.Date;
            }
            var filters = new List<Expression<Func<Schedule, bool>>>();
            filters.Add(FilterSchedule());
            var schedules = await _scheduleRepository.GetAsync(filters);

            return _mapper.Map<List<ScheduleResponse>>(schedules);
        }

        public async Task<List<ScheduleResponse>> SetSchedule(SetScheduleRequest request)
        {
            var doctorInfor = await _doctorInfoRepository.GetByIdAsync(request.DoctorId);

            Expression<Func<Schedule, bool>> FilterOldSchedule()
            {
                return x => x.DoctorId == request.DoctorId && x.Date == request.Date;
            }

            var oldSchedulefilters = new List<Expression<Func<Schedule, bool>>>();
            oldSchedulefilters.Add(FilterOldSchedule());
            var oldSchedule = await _scheduleRepository.GetAsync(oldSchedulefilters);
            var oldTimeSelect = oldSchedule.Select(x => x.TimeSelect);

            var addTimeSelects = request.TimeSelects.Except(oldTimeSelect);
            var deleteTimeSelects = oldTimeSelect.Except(request.TimeSelects);

            var addSchedules = new List<Schedule>();
            foreach (var item in addTimeSelects)
            {
                var schedule = _mapper.Map<Schedule>(request);
                //schedule.Price = doctorInfor.Price;
                schedule.TimeSelect = item;

                addSchedules.Add(schedule);
            }

            var deleteSchedules = new List<Schedule>();
            foreach (var item in deleteTimeSelects)
            {
                Expression<Func<Schedule, bool>> FilterDeleteSchedule()
                {
                    return x => x.DoctorId == request.DoctorId && x.Date == request.Date && x.TimeSelect == item;
                }
                var deleteSchedulefilters = new List<Expression<Func<Schedule, bool>>>();
                deleteSchedulefilters.Add(FilterDeleteSchedule());
                var schedule = (await _scheduleRepository.GetAsync(deleteSchedulefilters)).First();

                deleteSchedules.Add(schedule);
            }

            await _unitOfWork.ExecuteTransactionAsync(async () =>
            {
                await _scheduleRepository.AddRangeAsync(addSchedules);
                _scheduleRepository.DeleteRange(deleteSchedules);
            });

            Expression<Func<Schedule, bool>> FilterSchedule()
            {
                return x => x.DoctorId == request.DoctorId && x.Date == request.Date;
            }
            var filters = new List<Expression<Func<Schedule, bool>>>();
            filters.Add(FilterSchedule());
            var schedules = await _scheduleRepository.GetAsync(filters);

            return _mapper.Map<List<ScheduleResponse>>(schedules);
        }
    }
}
