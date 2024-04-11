using AutoMapper;
using BookingCare.Application.DTOs.Requests.Auth;
using BookingCare.Application.DTOs.Requests.Booking;
using BookingCare.Application.DTOs.Requests.Clinic;
using BookingCare.Application.DTOs.Requests.Contact;
using BookingCare.Application.DTOs.Requests.Doctor;
using BookingCare.Application.DTOs.Requests.Speciality;
using BookingCare.Application.DTOs.Requests.User;
using BookingCare.Application.DTOs.Responses;
using BookingCare.Application.DTOs.Responses.Address;
using BookingCare.Application.DTOs.Responses.Auth;
using BookingCare.Application.DTOs.Responses.Booking;
using BookingCare.Application.DTOs.Responses.Clinic;
using BookingCare.Application.DTOs.Responses.Contact;
using BookingCare.Application.DTOs.Responses.Doctor;
using BookingCare.Application.DTOs.Responses.Speciality;
using BookingCare.Application.DTOs.Responses.User;
using BookingCare.Domain.Common;
using BookingCare.Domain.Entities;
using BookingCare.Domain.Enums;
using System.Data;

namespace BookingCare.Application.Mappings
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile()
        {
            CreateMap<Province, ProvinceResponse>();
            CreateMap<District, DistrictResponse>();
            CreateMap<Ward, WardResponse>();


            CreateMap<SignUpRequest, AppUser>()
                .ForMember(a => a.UserName, b => b.MapFrom(a => a.Email));
            CreateMap<AppUser, AuthResponse>()
                .ForMember(a => a.Gender, b => b.MapFrom(a => EnumExtensions.GetDescription(a.Gender)));


            CreateMap<CreateUserRequest, AppUser>()
                .ForMember(a => a.UserName, b => b.MapFrom(a => a.Email));
            CreateMap<AppUser, UserResponse>()
                .ForMember(a => a.Gender, b => b.MapFrom(a => EnumExtensions.GetDescription(a.Gender)));
            CreateMap<AppUser, UserDetailResponse>()
                .ForMember(a => a.Gender, b => b.MapFrom(a => EnumExtensions.GetDescription(a.Gender)))
                .ForMember(a => a.WardName, b => b.MapFrom(c => c.Ward.WardName))
                .ForMember(a => a.DistrictName, b => b.MapFrom(c => c.Ward.District.DistrictName))
                .ForMember(a => a.ProvinceName, b => b.MapFrom(c => c.Ward.District.Province.ProvinceName));
            CreateMap<PaginationResponse<AppUser>, PaginationResponse<UserDetailResponse>>();


            CreateMap<CreateDoctorRequest, AppUser>()
                .ForMember(a => a.UserName, b => b.MapFrom(a => a.Email));
            CreateMap<CreateDoctorRequest, DoctorInfo>();
            CreateMap<UpdateDoctorRequest, DoctorInfo>();
            CreateMap<AppUser, DoctorInfoResponse>()
                .ForMember(a => a.Gender, b => b.MapFrom(a => EnumExtensions.GetDescription(a.Gender)))
                .ForMember(a => a.MaxPatient, b => b.MapFrom(c => c.DoctorInfo.MaxPatient))
                .ForMember(a => a.Image, b => b.MapFrom(c => c.DoctorInfo.Image))
                .ForMember(a => a.PositionId, b => b.MapFrom(c => c.DoctorInfo.PositionId))
                .ForMember(a => a.DescriptionId, b => b.MapFrom(c => c.DoctorInfo.DescriptionId))
                .ForMember(a => a.ClinicId, b => b.MapFrom(c => c.DoctorInfo.ClinicId))
                .ForMember(a => a.SpecialityId, b => b.MapFrom(c => c.DoctorInfo.SpecialityId))
            ;
            CreateMap<AppUser, DoctorInfoDetailResponse>()
                .ForMember(a => a.Gender, b => b.MapFrom(a => EnumExtensions.GetDescription(a.Gender)))
                .ForMember(a => a.MaxPatient, b => b.MapFrom(c => c.DoctorInfo.MaxPatient))
                .ForMember(a => a.Image, b => b.MapFrom(c => c.DoctorInfo.Image))
                .ForMember(a => a.WardName, b => b.MapFrom(c => c.Ward.WardName))
                .ForMember(a => a.DistrictName, b => b.MapFrom(c => c.Ward.District.DistrictName))
                .ForMember(a => a.ProvinceName, b => b.MapFrom(c => c.Ward.District.Province.ProvinceName))
                .ForMember(a => a.PositionId, b => b.MapFrom(c => c.DoctorInfo.PositionId))
                .ForMember(a => a.Position, b => b.MapFrom(c => c.DoctorInfo.PositionTextContent.Translations
                    .Where(x => x.LanguageId == Thread.CurrentThread.CurrentCulture.Name)
                    .First().Content))
                .ForMember(a => a.DescriptionId, b => b.MapFrom(c => c.DoctorInfo.DescriptionId))
                .ForMember(a => a.Description, b => b.MapFrom(c => c.DoctorInfo.DescriptionTextContent.Translations
                    .Where(x => x.LanguageId == Thread.CurrentThread.CurrentCulture.Name)
                    .First().Content))
                .ForMember(a => a.ClinicId, b => b.MapFrom(c => c.DoctorInfo.ClinicId))
                .ForMember(a => a.ClinicName, b => b.MapFrom(c => c.DoctorInfo.Clinic.Name))
                .ForMember(a => a.SpecialityId, b => b.MapFrom(c => c.DoctorInfo.SpecialityId))
                .ForMember(a => a.SpecialityName, b => b.MapFrom(c => c.DoctorInfo.Speciality.NameTextContent.Translations
                    .Where(x => x.LanguageId == Thread.CurrentThread.CurrentCulture.Name)
                    .First().Content))
            ;
            CreateMap<PaginationResponse<AppUser>, PaginationResponse<DoctorInfoDetailResponse>>();


            CreateMap<CreateClinicRequest, Clinic>();
            CreateMap<UpdateClinicRequest, Clinic>();
            CreateMap<Clinic, ClinicResponse>();
            CreateMap<Clinic, ClinicDetailResponse>()
                .ForMember(a => a.WardName, b => b.MapFrom(c => c.Ward.WardName))
                .ForMember(a => a.DistrictName, b => b.MapFrom(c => c.Ward.District.DistrictName))
                .ForMember(a => a.ProvinceName, b => b.MapFrom(c => c.Ward.District.Province.ProvinceName))
                .ForMember(a => a.Description, b => b.MapFrom(c => c.DescriptionTextContent.Translations
                    .Where(x => x.LanguageId == Thread.CurrentThread.CurrentCulture.Name)
                    .First().Content))
            ;
            CreateMap<Clinic, ClinicDetailForAdminResponse>()
                .ForMember(a => a.WardName, b => b.MapFrom(c => c.Ward.WardName))
                .ForMember(a => a.DistrictName, b => b.MapFrom(c => c.Ward.District.DistrictName))
                .ForMember(a => a.ProvinceName, b => b.MapFrom(c => c.Ward.District.Province.ProvinceName))
                .ForMember(a => a.ViDescription, b => b.MapFrom(c => c.DescriptionTextContent.Translations
                    .Where(x => x.LanguageId == LanguageCode.VietNamese)
                    .First().Content))
                .ForMember(a => a.EnDescription, b => b.MapFrom(c => c.DescriptionTextContent.Translations
                    .Where(x => x.LanguageId == LanguageCode.English)
                    .First().Content))
            ;
            CreateMap<Clinic, NameResponse>();
            CreateMap<PaginationResponse<Clinic>, PaginationResponse<ClinicDetailResponse>>();


            CreateMap<CreateSpecialityRequest, Speciality>();
            CreateMap<UpdateSpecialityRequest, Speciality>();
            CreateMap<Speciality, SpecialityResponse>();
            CreateMap<Speciality, SpecialityDetailResponse>()
                .ForMember(a => a.Name, b => b.MapFrom(c => c.NameTextContent.Translations
                .Where(x => x.LanguageId == Thread.CurrentThread.CurrentCulture.Name)
                    .First().Content))
                .ForMember(a => a.Description, b => b.MapFrom(c => c.DescriptionTextContent.Translations
                .Where(x => x.LanguageId == Thread.CurrentThread.CurrentCulture.Name)
                    .First().Content))
            ;
            CreateMap<Speciality, NameResponse>()
                .ForMember(a => a.Name, b => b.MapFrom(c => c.NameTextContent.Translations
                .Where(x => x.LanguageId == Thread.CurrentThread.CurrentCulture.Name)
                    .First().Content));
            CreateMap<PaginationResponse<Speciality>, PaginationResponse<SpecialityDetailResponse>>();


            CreateMap<SetScheduleRequest, Schedule>();
            CreateMap<Schedule, ScheduleResponse>()
                .ForMember(a => a.TimeSelect, b => b.MapFrom(a => EnumExtensions.GetListDescriptions<TimeSelectEnum>().FirstOrDefault(x => x.Key == (int)a.TimeSelect)));


            CreateMap<CreateBookingRequest, Booking>();
            CreateMap<Booking, BookingResponse>()
                .ForMember(a => a.Gender, b => b.MapFrom(a => EnumExtensions.GetDescription(a.Gender)))
                .ForMember(a => a.Status, b => b.MapFrom(a => EnumExtensions.GetDescription(a.Status)));
            CreateMap<Booking, BookingDetailResponse>()
                .ForMember(a => a.Gender, b => b.MapFrom(a => EnumExtensions.GetDescription(a.Gender)))
                .ForMember(a => a.Status, b => b.MapFrom(a => EnumExtensions.GetDescription(a.Status)))
                .ForMember(a => a.DoctorName, b => b.MapFrom(c => c.Doctor.Name))
                .ForMember(a => a.Date, b => b.MapFrom(c => c.Schedule.Date))
                .ForMember(a => a.TimeSelect, b => b.MapFrom(c => EnumExtensions.GetListDescriptions<TimeSelectEnum>().FirstOrDefault(x => x.Key == (int)c.Schedule.TimeSelect).Value));
            CreateMap<PaginationResponse<Booking>, PaginationResponse<BookingDetailResponse>>();

            CreateMap<CreateContactRequest, Contact>();
            CreateMap<Contact, ContactResponse>();
            CreateMap<PaginationResponse<Contact>, PaginationResponse<ContactResponse>>();
        }
    }
}