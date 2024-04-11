using BookingCare.Application.Repositories;
using BookingCare.Domain.Base;
using BookingCare.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace BookingCare.Application.DTOs.Requests.User
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
    {
        private readonly IStringLocalizer<UpdateUserValidator> _stringLocalizer;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWardRepository _wardRepository;
        public UpdateUserValidator
            (
                IStringLocalizer<UpdateUserValidator> stringLocalizer,
                UserManager<AppUser> userManager,
                IWardRepository wardRepository
            )
        {
            _stringLocalizer = stringLocalizer;
            _userManager = userManager;
            _wardRepository = wardRepository;

            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"])
                .MaximumLength(50)
                .WithMessage(_stringLocalizer["MaximumLengthValidator"])
                .WithName(_stringLocalizer["UserName"]);

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
                    })
                    .When(x => _userManager.Users.FirstOrDefault(y => y.Id == x.Id).PhoneNumber != x.PhoneNumber)
                    .WithMessage(_stringLocalizer["ExistValidator"])
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
                        ruleRole.Add(RoleName.Admin);
                        //ruleRole.Add(RoleName.DOCTOR);
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
        }
    }
}