using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookingCare.Application.DTOs.Requests.Contact;

public class CreateContactValidator : AbstractValidator<CreateContactRequest>
{
    private readonly IStringLocalizer<CreateContactValidator> _stringLocalizer;

    public CreateContactValidator
        (
            IStringLocalizer<CreateContactValidator> stringLocalizer
        )
    {
        _stringLocalizer = stringLocalizer;

        RuleFor(x => x.FullName)
            .NotNull()
            .NotEmpty()
            .WithMessage(_stringLocalizer["NotNullValidator"])
            .MaximumLength(30)
            .WithMessage(_stringLocalizer["MaximumLengthValidator"])
            .WithName(_stringLocalizer["FullName"]);

        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty()
            .WithMessage(_stringLocalizer["NotNullValidator"])
            .EmailAddress()
            .WithMessage(_stringLocalizer["EmailValidator"]);

        RuleFor(x => x.Message)
            .NotNull()
            .NotEmpty()
            .WithMessage(_stringLocalizer["NotNullValidator"])
            .WithName(_stringLocalizer["Message"]);
    }
}
