using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookingCare.Application.DTOs.Requests.Account
{
    public class ResetPasswordValidator : AbstractValidator<ResetPasswordRequest>
    {
        private readonly IStringLocalizer<ResetPasswordValidator> _stringLocalizer;
        public ResetPasswordValidator(IStringLocalizer<ResetPasswordValidator> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;

            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"])
                .EmailAddress()
                .WithMessage(_stringLocalizer["EmailValidator"]);

            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"])
                .MinimumLength(6)
                .WithMessage(_stringLocalizer["MinimumLengthValidator"])
                .WithName(_stringLocalizer["PassWord"]);

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithMessage(_stringLocalizer["EqualValidator"])
                .WithName(_stringLocalizer["ConfirmPassword"]);

            RuleFor(x => x.Code)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"]);
        }
    }
}