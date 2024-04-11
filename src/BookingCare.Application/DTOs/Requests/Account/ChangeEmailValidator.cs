using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookingCare.Application.DTOs.Requests.Account
{
    public class ChangeEmailValidator : AbstractValidator<ChangeEmailRequest>
    {
        private readonly IStringLocalizer<ChangeEmailValidator> _stringLocalizer;
        public ChangeEmailValidator
            (
                IStringLocalizer<ChangeEmailValidator> stringLocalizer
            )
        {
            _stringLocalizer = stringLocalizer;

            RuleFor(x => x.UserId)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"]);

            RuleFor(x => x.NewEmail)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"])
                .EmailAddress()
                .WithMessage(_stringLocalizer["EmailValidator"]);

            RuleFor(x => x.Code)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"]);
        }
    }
}