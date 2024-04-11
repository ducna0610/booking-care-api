using BookingCare.Application.Repositories;
using BookingCare.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace BookingCare.Application.DTOs.Requests.Doctor
{
    public class CreateDoctorValidator : AbstractValidator<CreateDoctorRequest>
    {
        private readonly IStringLocalizer<CreateDoctorValidator> _stringLocalizer;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWardRepository _wardRepository;
        private readonly IClinicRepository _clinicRepository;
        private readonly ISpecialityRepository _specialityRepository;
        public CreateDoctorValidator
            (
                IStringLocalizer<CreateDoctorValidator> stringLocalizer,
                UserManager<AppUser> userManager,
                IWardRepository wardRepository,
                IClinicRepository clinicRepository,
                ISpecialityRepository specialityRepository

            )
        {
            _stringLocalizer = stringLocalizer;
            _userManager = userManager;
            _wardRepository = wardRepository;
            _clinicRepository = clinicRepository;
            _specialityRepository = specialityRepository;

            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"])
                .MaximumLength(50)
                .WithMessage(_stringLocalizer["MaximumLengthValidator"])
                .WithName(_stringLocalizer["UserName"]);

            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"])
                .EmailAddress()
                .WithMessage(_stringLocalizer["EmailValidator"]);

            RuleFor(x => x.Address)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"])
                .MaximumLength(50)
                .WithMessage(_stringLocalizer["MaximumLengthValidator"])
                .WithName(_stringLocalizer["Address"]);

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^0\d{9}$")
                .WithMessage(_stringLocalizer["RegularExpressionValidator"])
                .WithName(_stringLocalizer["PhoneNumber"]);

            When(x => x.PhoneNumber != null, () =>
            {
                RuleFor(x => x.PhoneNumber)
                    .Must((phoneNumber) =>
                    {
                        return _userManager.Users.FirstOrDefault(x => x.PhoneNumber.Equals(phoneNumber)) == null;
                    }).WithMessage(_stringLocalizer["ExistValidator"])
                    .WithName(_stringLocalizer["PhoneNumber"]);
            });

            RuleFor(x => x.Gender)
                .IsInEnum()
                .WithMessage(_stringLocalizer["NotValidValidator"])
                .WithName(_stringLocalizer["Gender"]);

            RuleFor(x => x.WardId)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"])
                .WithName(_stringLocalizer["Ward"]);

            When(x => x.WardId != null, () =>
            {
                RuleFor(x => x.WardId)
                    .Must((id) =>
                    {
                        if (_wardRepository.GetById(id) == null)
                            return false;
                        return true;
                    }).WithMessage(_stringLocalizer["NotExistValidator"])
                    .WithName(_stringLocalizer["Ward"]);
            });

            RuleFor(x => x.EnPosition)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"])
                .WithName(_stringLocalizer["EnPosition"]);

            RuleFor(x => x.ViPosition)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"])
                .WithName(_stringLocalizer["ViPosition"]);

            RuleFor(x => x.EnDescription)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"])
                .WithName(_stringLocalizer["EnDescription"]);

            RuleFor(x => x.ViDescription)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"])
                .WithName(_stringLocalizer["ViDescription"]);

            RuleFor(x => x.ClinicId)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"])
                .WithName(_stringLocalizer["Clinic"]);

            When(x => x.ClinicId != null, () =>
            {
                RuleFor(x => x.ClinicId)
                    .Must((id) =>
                    {
                        if (_clinicRepository.GetById(id) == null)
                            return false;
                        return true;
                    }).WithMessage(_stringLocalizer["NotExistValidator"])
                    .WithName(_stringLocalizer["Clinic"]);
            });

            RuleFor(x => x.SpecialityId)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"])
                .WithName(_stringLocalizer["Speciality"]);

            When(x => x.SpecialityId != null, () =>
            {
                RuleFor(x => x.SpecialityId)
                    .Must((id) =>
                    {
                        if (_specialityRepository.GetById(id) == null)
                            return false;
                        return true;
                    }).WithMessage(_stringLocalizer["NotExistValidator"])
                    .WithName(_stringLocalizer["Speciality"]);
            });

            RuleFor(x => x.MaxPatient)
                .InclusiveBetween(1, 10)
                .WithMessage(_stringLocalizer["InclusiveBetweenValidator"])
                .WithName(_stringLocalizer["MaxPatient"]);

            RuleFor(x => x.Price)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"])
                .WithName(_stringLocalizer["Price"]);

            RuleFor(x => x.Image)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"])
                .WithName(_stringLocalizer["Image"]);

            When(x => x.Image != null, () =>
            {
                RuleFor(x => x.Image.ContentType)
                    .Must(x => x.Equals("image/jpeg") || x.Equals("image/jpg") || x.Equals("image/png"))
                    .WithMessage(_stringLocalizer["NotImageValidator"])
                .WithName(_stringLocalizer["Image"]);
            });
        }
    }
}