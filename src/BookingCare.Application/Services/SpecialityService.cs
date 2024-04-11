using AutoMapper;
using BookingCare.Application.DTOs.Requests.Speciality;
using BookingCare.Application.DTOs.Responses;
using BookingCare.Application.DTOs.Responses.Speciality;
using BookingCare.Application.Repositories;
using BookingCare.Application.Utils;
using BookingCare.Domain.Common;
using BookingCare.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using System.Linq.Expressions;

namespace BookingCare.Application.Services
{
    public interface ISpecialityService
    {
        Task<PaginationResponse<SpecialityDetailResponse>> ToPagination(PaginationSpecialityRequest request);
        Task<IEnumerable<SpecialityDetailResponse>> GetAll();
        Task<IEnumerable<NameResponse>> GetListName();
        Task<SpecialityResponse> GetById(Guid id);
        Task<SpecialityResponse> Create(CreateSpecialityRequest request);
        Task<SpecialityResponse> Update(Guid id, UpdateSpecialityRequest request);
        Task Delete(Guid id);
        Task<IEnumerable<SpecialityResponse>> Import(IFormFile file);
    }

    public class SpecialityService : ISpecialityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SpecialityService> _stringLocalizer;
        private readonly IFileStorageService _fileStorageService;
        private readonly IExcelService _excelService;
        private readonly ITextContextRepository _textContextRepository;
        private readonly ITranslationRepository _translationRepository;
        private readonly ISpecialityRepository _specialityRepository;

        public SpecialityService
            (
                IUnitOfWork unitOfWork,
                IMapper mapper,
                IStringLocalizer<SpecialityService> stringLocalizer,
                IFileStorageService fileStorageService,
                IExcelService excelService,
                ITextContextRepository textContextRepository,
                ITranslationRepository translationRepository,
                ISpecialityRepository specialityRepository
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
            _fileStorageService = fileStorageService;
            _excelService = excelService;
            _textContextRepository = textContextRepository;
            _translationRepository = translationRepository;
            _specialityRepository = specialityRepository;
        }

        public async Task<PaginationResponse<SpecialityDetailResponse>> ToPagination(PaginationSpecialityRequest request)
        {
            Expression<Func<Speciality, object>> name = x => x.NameTextContent.Translations;
            Expression<Func<Speciality, object>> description = x => x.DescriptionTextContent.Translations;

            var includes = new List<Expression<Func<Speciality, object>>>();
            includes.Add(name);
            includes.Add(description);

            var result = await _specialityRepository.ToPaginationAsync(request.PageIndex, request.PageSize, includes: includes);

            return _mapper.Map<PaginationResponse<SpecialityDetailResponse>>(result);
        }

        public async Task<IEnumerable<SpecialityDetailResponse>> GetAll()
        {
            Expression<Func<Speciality, object>> name = x => x.NameTextContent.Translations;
            Expression<Func<Speciality, object>> description = x => x.DescriptionTextContent.Translations;

            var includes = new List<Expression<Func<Speciality, object>>>();
            includes.Add(name);
            includes.Add(description);

            var specialities = await _specialityRepository.GetAsync(includes: includes);
            return _mapper.Map<List<SpecialityDetailResponse>>(specialities);
        }

        public async Task<IEnumerable<NameResponse>> GetListName()
        {
            Expression<Func<Speciality, object>> name = x => x.NameTextContent.Translations;

            var includes = new List<Expression<Func<Speciality, object>>>();
            includes.Add(name);

            var specialities = await _specialityRepository.GetAsync(includes: includes);

            return _mapper.Map<List<NameResponse>>(specialities);
        }

        public async Task<SpecialityResponse> GetById(Guid id)
        {
            var clinic = await _specialityRepository.GetByIdAsync(id);
            return _mapper.Map<SpecialityResponse>(clinic);
        }

        public async Task<SpecialityResponse> Create(CreateSpecialityRequest request)
        {
            var nameTranslations = new List<Translation>();
            var nameTextContext = new TextContent()
            {
                OriginalText = request.EnName,
            };
            nameTranslations.Add(
                new Translation()
                {
                    TextContextId = nameTextContext.Id,
                    LanguageId = LanguageCode.English,
                    Content = request.EnName
                }
            );
            nameTranslations.Add(
                new Translation()
                {
                    TextContextId = nameTextContext.Id,
                    LanguageId = LanguageCode.VietNamese,
                    Content = request.ViName
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

            var speciality = _mapper.Map<Speciality>(request);
            speciality.NameId = nameTextContext.Id;
            speciality.DescriptionId = descriptionTextContext.Id;

            if (request.Image != null)
            {
                speciality.Image = await _fileStorageService.UploadAsync(request.Image);
            }


            await _unitOfWork.ExecuteTransactionAsync(async () =>
            {
                await _textContextRepository.AddAsync(nameTextContext);
                await _translationRepository.AddRangeAsync(nameTranslations);
                await _textContextRepository.AddAsync(descriptionTextContext);
                await _translationRepository.AddRangeAsync(descriptionTranslations);
                await _specialityRepository.AddAsync(speciality);
            });

            return _mapper.Map<SpecialityResponse>(speciality);
        }

        public async Task<SpecialityResponse> Update(Guid id, UpdateSpecialityRequest request)
        {
            var oldSpeciality = await _specialityRepository.GetByIdAsync(id);
            if (oldSpeciality != null)
            {
                throw new Exception(string.Format(_stringLocalizer["NotFound"], _stringLocalizer["Speciality"]));
            }

            var speciality = _mapper.Map<Speciality>(request);
            speciality.Id = id;
            speciality.NameId = oldSpeciality.NameId;
            speciality.DescriptionId = oldSpeciality.DescriptionId;
            speciality.CreatedAt = oldSpeciality.CreatedAt;
            speciality.Image = oldSpeciality.Image;

            if (request.NewImage != null)
            {
                await _fileStorageService.DeleteAsync(speciality.Image);
                speciality.Image = await _fileStorageService.UploadAsync(request.NewImage);
            }

            Expression<Func<Translation, bool>> enFilterByNameId()
            {
                return x => x.TextContextId == oldSpeciality.NameId && x.LanguageId == LanguageCode.English;
            }
            var enFilterByNameIdTranslations = new List<Expression<Func<Translation, bool>>>();
            enFilterByNameIdTranslations.Add(enFilterByNameId());
            var enNameTranslation = (await _translationRepository.GetAsync(enFilterByNameIdTranslations)).First();
            enNameTranslation.Content = request.EnName;

            Expression<Func<Translation, bool>> viFilterByNameId()
            {
                return x => x.TextContextId == oldSpeciality.NameId && x.LanguageId == LanguageCode.VietNamese;
            }
            var viFilterByNameIdTranslations = new List<Expression<Func<Translation, bool>>>();
            viFilterByNameIdTranslations.Add(viFilterByNameId());
            var viNameTranslation = (await _translationRepository.GetAsync(viFilterByNameIdTranslations)).First();
            viNameTranslation.Content = request.ViName;

            Expression<Func<Translation, bool>> enFilterByDescriptionId()
            {
                return x => x.TextContextId == oldSpeciality.DescriptionId && x.LanguageId == LanguageCode.English;
            }
            var enFilterByDescriptionIdTranslations = new List<Expression<Func<Translation, bool>>>();
            enFilterByDescriptionIdTranslations.Add(enFilterByDescriptionId());
            var enDescriptionTranslation = (await _translationRepository.GetAsync(enFilterByDescriptionIdTranslations)).First();
            enDescriptionTranslation.Content = request.EnDescription;


            Expression<Func<Translation, bool>> viFilterByDescriptionId()
            {
                return x => x.TextContextId == oldSpeciality.DescriptionId && x.LanguageId == LanguageCode.VietNamese;
            }
            var viFilterByDescriptionIdTranslations = new List<Expression<Func<Translation, bool>>>();
            viFilterByDescriptionIdTranslations.Add(viFilterByDescriptionId());
            var viDescriptionTranslation = (await _translationRepository.GetAsync(viFilterByDescriptionIdTranslations)).First();
            viDescriptionTranslation.Content = request.ViDescription;

            await _unitOfWork.ExecuteTransactionAsync(() =>
            {
                _specialityRepository.Update(speciality);
                _translationRepository.Update(enNameTranslation);
                _translationRepository.Update(viNameTranslation);
                _translationRepository.Update(enDescriptionTranslation);
                _translationRepository.Update(viDescriptionTranslation);
            });

            return _mapper.Map<SpecialityResponse>(speciality);
        }

        public async Task Delete(Guid id)
        {
            var speciality = await _specialityRepository.GetByIdAsync(id);

            await _fileStorageService.DeleteAsync(speciality.Image);

            await _unitOfWork.ExecuteTransactionAsync(async () =>
            {
                _specialityRepository.Delete(speciality);
            });
        }

        public async Task<IEnumerable<SpecialityResponse>> Import(IFormFile file)
        {
            var fileName = await _fileStorageService.UploadAsync(file);

            var createSpecialityRequests = await _excelService.ImportAsync<CreateSpecialityRequest>(fileName);

            var result = new List<SpecialityResponse>();
            foreach (var createSpecialityRequest in createSpecialityRequests)
            {
                result.Add(await Create(createSpecialityRequest));
            }

            return result;
        }
    }
}