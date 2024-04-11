using BookingCare.Application.DTOs.Requests.Auth;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookingCare.Application.DTOs.Requests.Account
{
    public class SendMailChangeEmailValidator : AbstractValidator<SendMailChangeEmailRequest>
    {
        private readonly IStringLocalizer<SignUpValidator> _stringLocalizer;
        public SendMailChangeEmailValidator
            (
                IStringLocalizer<SignUpValidator> stringLocalizer
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
        }
    }
}