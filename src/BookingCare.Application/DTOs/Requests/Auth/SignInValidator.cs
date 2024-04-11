using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookingCare.Application.DTOs.Requests.Auth
{
    public class SignInValidator : AbstractValidator<SignInRequest>
    {
        private readonly IStringLocalizer<SignInValidator> _stringLocalizer;
        public SignInValidator(IStringLocalizer<SignInValidator> stringLocalizer)
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
                .WithName(_stringLocalizer["PassWord"]);
        }
    }
}