using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookingCare.Application.DTOs.Requests.Auth
{
    public class TokenValidator : AbstractValidator<TokenRequest>
    {
        private readonly IStringLocalizer<TokenValidator> _stringLocalizer;
        public TokenValidator(IStringLocalizer<TokenValidator> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;

            RuleFor(x => x.AccessToken)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"]);

            RuleFor(x => x.RefreshToken)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"]);
        }
    }
}