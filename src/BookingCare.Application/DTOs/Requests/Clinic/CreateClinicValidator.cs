using BookingCare.Application.Repositories;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookingCare.Application.DTOs.Requests.Clinic;

public class CreateClinicValidator : AbstractValidator<CreateClinicRequest>
{
    private readonly IStringLocalizer<CreateClinicValidator> _stringLocalizer;
    private readonly IWardRepository _wardRepository;

    public CreateClinicValidator
        (
            IStringLocalizer<CreateClinicValidator> stringLocalizer,
            IWardRepository wardRepository
        )
    {
        _stringLocalizer = stringLocalizer;
        _wardRepository = wardRepository;

        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage(_stringLocalizer["NotNullValidator"])
            .MaximumLength(30)
            .WithMessage(_stringLocalizer["MaximumLengthValidator"])
            .WithName(_stringLocalizer["ClinicName"]);

        RuleFor(x => x.EnDescription)
            .NotNull()
            .NotEmpty()
            .WithMessage(_stringLocalizer["NotNullValidator"])
            .WithName(_stringLocalizer["EnDescription"]);

        RuleFor(x => x.ViDescription)
            .NotNull()
            .NotEmpty()
            .WithMessage(_stringLocalizer["NotNullValidator"])
            .WithName(_stringLocalizer["ViDescription"]);

        RuleFor(x => x.Address)
            .NotNull()
            .NotEmpty()
            .WithMessage(_stringLocalizer["NotNullValidator"])
            .MaximumLength(30)
            .WithMessage(_stringLocalizer["MaximumLengthValidator"])
            .WithName(_stringLocalizer["Address"]);

        RuleFor(x => x.Image)
            .NotNull()
            .NotEmpty()
            .WithMessage(_stringLocalizer["NotNullValidator"])
            .WithName(_stringLocalizer["Image"]);

        When(x => x.Image != null, () =>
        {
            RuleFor(x => x.Image.ContentType)
                .Must(x => x.Equals("image/jpeg") || x.Equals("image/jpg") || x.Equals("image/png"))
                .WithMessage(_stringLocalizer["NotImageValidator"])
            .OverridePropertyName(_stringLocalizer["Image"]);
        });

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
    }
}