using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookingCare.Application.DTOs.Requests.Auth
{
    public class ConfirmEmailValidator : AbstractValidator<ConfirmEmailRequest>
    {
        private readonly IStringLocalizer<ConfirmEmailValidator> _stringLocalizer;
        public ConfirmEmailValidator(IStringLocalizer<ConfirmEmailValidator> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;

            RuleFor(x => x.UserId)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"]);

            RuleFor(x => x.Token)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"]);
        }
    }
}