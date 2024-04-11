using BookingCare.Application.Repositories;
using BookingCare.Domain.Base;
using BookingCare.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace BookingCare.Application.DTOs.Requests.Auth
{
    public class SignUpValidator : AbstractValidator<SignUpRequest>
    {
        private readonly IStringLocalizer<SignUpValidator> _stringLocalizer;
        private readonly IAppUserRepository _appUserRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWardRepository _wardRepository;
        public SignUpValidator
            (
                IStringLocalizer<SignUpValidator> stringLocalizer,
                IAppUserRepository appUserRepository,
                UserManager<AppUser> userManager,
                IWardRepository wardRepository
            )
        {
            _stringLocalizer = stringLocalizer;
            _appUserRepository = appUserRepository;
            _userManager = userManager;
            _wardRepository = wardRepository;

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
                        //Expression<Func<AppUser, bool>> FilterByPhoneNumber()
                        //{
                        //    return x =>
                        //        x.PhoneNumber.Contains(phoneNumber);
                        //}

                        //var filters = new List<Expression<Func<AppUser, bool>>>();
                        //filters.Add(FilterByPhoneNumber());

                        //if (_appUserRepository.Any(filters))
                        //    return false;
                        //return true;

                        return _userManager.Users.FirstOrDefault(x => x.PhoneNumber.Equals(phoneNumber)) == null;
                    }).WithMessage(_stringLocalizer["ExistValidator"])
                    .WithName(_stringLocalizer["PhoneNumber"]);
            });

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"])
                .MinimumLength(6)
                .WithMessage(_stringLocalizer["MinimumLengthValidator"])
                .WithName(_stringLocalizer["PassWord"]);

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

            RuleFor(x => x.Roles)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"])
                .WithName(_stringLocalizer["Roles"]);

            When(x => x.Roles != null, () =>
            {
                RuleFor(x => x.Roles)
                    .Must(roles =>
                    {
                        var ruleRole = new List<string>() { };
                        ruleRole.Add(RoleName.Patient);

                        foreach (var role in roles)
                        {
                            if (!ruleRole.Contains(role))
                            {
                                return false;
                            }
                        }
                        return true;
                    })
                    .WithMessage(_stringLocalizer["NotValidValidator"])
                    .WithName(_stringLocalizer["Roles"]);
            });

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithMessage(_stringLocalizer["EqualValidator"])
                .WithName(_stringLocalizer["ConfirmPassword"]);
        }
    }
}