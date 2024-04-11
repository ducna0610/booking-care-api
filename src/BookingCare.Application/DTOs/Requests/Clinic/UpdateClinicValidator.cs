using BookingCare.Application.Repositories;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookingCare.Application.DTOs.Requests.Clinic
{
    public class UpdateClinicValidator : AbstractValidator<UpdateClinicRequest>
    {
        private readonly IWardRepository _wardRepository;
        private readonly IStringLocalizer<UpdateClinicValidator> _stringLocalizer;

        public UpdateClinicValidator(IStringLocalizer<UpdateClinicValidator> stringLocalizer, IWardRepository wardRepository)
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

            When(x => x.NewImage != null, () =>
            {
                RuleFor(x => x.NewImage.ContentType)
                    .Must(x => x.Equals("image/jpeg") || x.Equals("image/jpg") || x.Equals("image/png"))
                    .WithMessage(_stringLocalizer["NotImageValidator"])
                .WithName(_stringLocalizer["Image"]);
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
}