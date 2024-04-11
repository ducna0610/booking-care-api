using BookingCare.Application.DTOs.Requests.Clinic;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookingCare.Application.DTOs.Requests.Speciality
{
    public class CreateSpecialityValidator : AbstractValidator<CreateSpecialityRequest>
    {
        private readonly IStringLocalizer<CreateSpecialityValidator> _stringLocalizer;
        public CreateSpecialityValidator(IStringLocalizer<CreateSpecialityValidator> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;

            RuleFor(x => x.ViName)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"])
                .WithName(_stringLocalizer["ViName"]);

            RuleFor(x => x.EnName)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"])
                .WithName(_stringLocalizer["EnName"]);

            RuleFor(x => x.ViDescription)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"])
                .WithName(_stringLocalizer["ViDescription"]);

            RuleFor(x => x.EnDescription)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"])
                .WithName(_stringLocalizer["EnDescription"]);

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
                .WithName(_stringLocalizer["Image"]);
            });
        }
    }
}