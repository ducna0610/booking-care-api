using AutoMapper;
using BookingCare.Application.DTOs.Requests.Clinic;
using BookingCare.Application.DTOs.Responses;
using BookingCare.Application.DTOs.Responses.Clinic;
using BookingCare.Application.Repositories;
using BookingCare.Application.Utils;
using BookingCare.Domain.Common;
using BookingCare.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;

namespace BookingCare.Application.Services
{
    public interface IClinicService
    {
        Task<PaginationResponse<ClinicDetailResponse>> ToPagination(PaginationClinicRequest request);
        Task<IEnumerable<ClinicDetailResponse>> GetAll();
        Task<IEnumerable<NameResponse>> GetListName();
        Task<ClinicDetailResponse> GetById(Guid id);
        Task<ClinicDetailForAdminResponse> GetByIdForAdmin(Guid id);
        Task<ClinicResponse> Create(CreateClinicRequest request);
        Task<ClinicResponse> Update(Guid id, UpdateClinicRequest request);
        Task Delete(Guid id);
        Task<IEnumerable<ClinicResponse>> Import(IFormFile file);
        Task<string> Export();
    }

    public class ClinicService : IClinicService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ClinicService> _stringLocalizer;
        private readonly IFileStorageService _fileStorageService;
        private readonly IExcelService _excelService;
        private readonly ITextContextRepository _textContextRepository;
        private readonly ITranslationRepository _translationRepository;
        private readonly IClinicRepository _clinicRepository;

        public ClinicService
            (
                IUnitOfWork unitOfWork,
                IMapper mapper,
                IStringLocalizer<ClinicService> stringLocalizer,
                IFileStorageService fileStorageService,
                IExcelService excelService,
                ITextContextRepository textContextRepository,
                ITranslationRepository translationRepository,
                IClinicRepository clinicRepository
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
            _fileStorageService = fileStorageService;
            _excelService = excelService;
            _textContextRepository = textContextRepository;
            _translationRepository = translationRepository;
            _clinicRepository = clinicRepository;
        }

        public async Task<PaginationResponse<ClinicDetailResponse>> ToPagination(PaginationClinicRequest request)
        {
            //Expression<Func<Clinic, bool>> FilterByKeyWord()
            //{
            //    return x =>
            //        x.Name.Contains(request.KeyWord) ||
            //        x.Address.Contains(request.KeyWord);
            //}

            //Expression<Func<Clinic, bool>> FilterByName()
            //{
            //    return x =>
            //        x.Name.Contains(request.Name);
            //}

            //Expression<Func<Clinic, bool>> FilterByWardId()
            //{
            //    return x =>
            //        request.WardId == null ? true : x.WardId == request.WardId;
            //}

            //var filters = new List<Expression<Func<Clinic, bool>>>();
            //filters.Add(FilterByKeyWord());
            //filters.Add(FilterByName());
            //filters.Add(FilterByWardId());

            Expression<Func<Clinic, object>> ward = x => x.Ward;
            Expression<Func<Clinic, object>> district = x => x.Ward.District;
            Expression<Func<Clinic, object>> province = x => x.Ward.District.Province;
            Expression<Func<Clinic, object>> description = x => x.DescriptionTextContent.Translations;

            var includes = new List<Expression<Func<Clinic, object>>>();
            includes.Add(ward);
            includes.Add(district);
            includes.Add(province);
            includes.Add(description);

            var orders = new List<Func<IQueryable<Clinic>, IOrderedQueryable<Clinic>>>();

            //if (request.Orders != null && request.Orders.Count() > 0)
            //{
            //    foreach (var item in request.Orders)
            //    {
            //        Func<IQueryable<Clinic>, IOrderedQueryable<Clinic>> order = null;

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

            var result = await _clinicRepository.ToPaginationAsync(request.PageIndex, request.PageSize, null, includes, orders);

            return _mapper.Map<PaginationResponse<ClinicDetailResponse>>(result);
        }

        public async Task<IEnumerable<ClinicDetailResponse>> GetAll()
        {
            Expression<Func<Clinic, object>> ward = x => x.Ward;
            Expression<Func<Clinic, object>> district = x => x.Ward.District;
            Expression<Func<Clinic, object>> province = x => x.Ward.District.Province;
            Expression<Func<Clinic, object>> description = x => x.DescriptionTextContent.Translations;

            var includes = new List<Expression<Func<Clinic, object>>>();
            includes.Add(ward);
            includes.Add(district);
            includes.Add(province);
            includes.Add(description);

            var clinics = await _clinicRepository.GetAsync(includes: includes);
            return _mapper.Map<List<ClinicDetailResponse>>(clinics);
        }

        public async Task<IEnumerable<NameResponse>> GetListName()
        {
            var clinics = await _clinicRepository.GetAsync();

            return _mapper.Map<List<NameResponse>>(clinics);
        }

        public async Task<ClinicDetailResponse> GetById(Guid id)
        {
            Expression<Func<Clinic, bool>> FilterById()
            {
                return x => x.Id == id;
            }

            var filters = new List<Expression<Func<Clinic, bool>>>();
            filters.Add(FilterById());

            Expression<Func<Clinic, object>> ward = x => x.Ward;
            Expression<Func<Clinic, object>> district = x => x.Ward.District;
            Expression<Func<Clinic, object>> province = x => x.Ward.District.Province;
            Expression<Func<Clinic, object>> description = x => x.DescriptionTextContent.Translations;

            var includes = new List<Expression<Func<Clinic, object>>>();
            includes.Add(ward);
            includes.Add(district);
            includes.Add(province);
            includes.Add(description);

            var clinic = await _clinicRepository.GetAsync(filters, includes);

            if (clinic.Count() > 0)
            {
                return _mapper.Map<ClinicDetailResponse>(clinic.First());
            }
            return new ClinicDetailResponse();
        }

        public async Task<ClinicDetailForAdminResponse> GetByIdForAdmin(Guid id)
        {
            Expression<Func<Clinic, bool>> FilterById()
            {
                return x => x.Id == id;
            }

            var filters = new List<Expression<Func<Clinic, bool>>>();
            filters.Add(FilterById());

            Expression<Func<Clinic, object>> ward = x => x.Ward;
            Expression<Func<Clinic, object>> district = x => x.Ward.District;
            Expression<Func<Clinic, object>> province = x => x.Ward.District.Province;
            Expression<Func<Clinic, object>> description = x => x.DescriptionTextContent.Translations;

            var includes = new List<Expression<Func<Clinic, object>>>();
            includes.Add(ward);
            includes.Add(district);
            includes.Add(province);
            includes.Add(description);

            var clinic = await _clinicRepository.GetAsync(filters, includes);

            if (clinic.Count() > 0)
            {
                return _mapper.Map<ClinicDetailForAdminResponse>(clinic.First());
            }
            return new ClinicDetailForAdminResponse();
        }

        public async Task<ClinicResponse> Create(CreateClinicRequest request)
        {
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

            var clinic = _mapper.Map<Clinic>(request);
            clinic.DescriptionId = descriptionTextContext.Id;

            if (request.Image != null)
            {
                clinic.Image = await _fileStorageService.UploadAsync(request.Image);
            }

            await _unitOfWork.ExecuteTransactionAsync(async () =>
            {
                await _textContextRepository.AddAsync(descriptionTextContext);
                await _translationRepository.AddRangeAsync(descriptionTranslations);
                await _clinicRepository.AddAsync(clinic);
            });

            return _mapper.Map<ClinicResponse>(clinic);
        }

        public async Task<ClinicResponse> Update(Guid id, UpdateClinicRequest request)
        {
            var oldClinic = await _clinicRepository.GetByIdAsync(id);
            if (oldClinic == null)
            {
                throw new Exception(string.Format(_stringLocalizer["NotFound"], _stringLocalizer["Clinic"]));
            }

            var clinic = _mapper.Map<Clinic>(request);
            clinic.Id = id;
            clinic.DescriptionId = oldClinic.DescriptionId;
            clinic.Image = oldClinic.Image;
            clinic.CreatedAt = oldClinic.CreatedAt;

            if (request.NewImage != null)
            {
                await _fileStorageService.DeleteAsync(clinic.Image);
                clinic.Image = await _fileStorageService.UploadAsync(request.NewImage);
            }

            Expression<Func<Translation, bool>> enFilterByDescriptionId()
            {
                return x => x.TextContextId == oldClinic.DescriptionId && x.LanguageId == LanguageCode.English;
            }
            var enFilterByDescriptionIdTranslations = new List<Expression<Func<Translation, bool>>>();
            enFilterByDescriptionIdTranslations.Add(enFilterByDescriptionId());
            var enDescriptionTranslation = (await _translationRepository.GetAsync(enFilterByDescriptionIdTranslations)).First();
            enDescriptionTranslation.Content = request.EnDescription;


            Expression<Func<Translation, bool>> viFilterByDescriptionId()
            {
                return x => x.TextContextId == oldClinic.DescriptionId && x.LanguageId == LanguageCode.VietNamese;
            }
            var viFilterByDescriptionIdTranslations = new List<Expression<Func<Translation, bool>>>();
            viFilterByDescriptionIdTranslations.Add(enFilterByDescriptionId());
            var viDescriptionTranslation = (await _translationRepository.GetAsync(viFilterByDescriptionIdTranslations)).First();
            viDescriptionTranslation.Content = request.ViDescription;


            await _unitOfWork.ExecuteTransactionAsync(() =>
            {
                _clinicRepository.Update(clinic);
                _translationRepository.Update(enDescriptionTranslation);
                _translationRepository.Update(viDescriptionTranslation);
            });

            return _mapper.Map<ClinicResponse>(clinic);
        }

        public async Task Delete(Guid id)
        {
            var clinic = await _clinicRepository.GetByIdAsync(id);

            await _fileStorageService.DeleteAsync(clinic.Image);

            await _unitOfWork.ExecuteTransactionAsync(() =>
            {
                _clinicRepository.Delete(clinic);
            });
        }

        public async Task<IEnumerable<ClinicResponse>> Import(IFormFile file)
        {
            var fileName = await _fileStorageService.UploadAsync(file);

            var createClinicRequests = await _excelService.ImportAsync<CreateClinicRequest>(fileName);

            var result = new List<ClinicResponse>();
            foreach (var createClinicRequest in createClinicRequests)
            {
                result.Add(await Create(createClinicRequest));
            }

            return result;
        }

        public async Task<string> Export()
        {
            var clinics = await GetAll();
            return await _excelService.ExportAsync(clinics);
        }
    }
}
