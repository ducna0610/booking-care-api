using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookingCare.Application.DTOs.Requests.Account
{
    public class SendMailResetPasswordValidator : AbstractValidator<SendMailResetPasswordRequest>
    {
        private readonly IStringLocalizer<SendMailResetPasswordValidator> _stringLocalizer;
        public SendMailResetPasswordValidator(IStringLocalizer<SendMailResetPasswordValidator> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;

            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"])
                .EmailAddress()
                .WithMessage(_stringLocalizer["EmailValidator"]);
        }
    }
}