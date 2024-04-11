using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookingCare.Application.DTOs.Requests.Account
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordRequest>
    {
        private readonly IStringLocalizer<ChangePasswordValidator> _stringLocalizer;
        public ChangePasswordValidator(IStringLocalizer<ChangePasswordValidator> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;

            RuleFor(x => x.NewPassWord)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"]);

            RuleFor(x => x.OldPassWord)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"]);
        }
    }
}