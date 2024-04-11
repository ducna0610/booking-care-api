using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookingCare.Application.DTOs.Requests.Doctor
{
    public class GetScheduleValidator : AbstractValidator<GetScheduleRequest>
    {
        private readonly IStringLocalizer<CreateDoctorValidator> _stringLocalizer;
        public GetScheduleValidator
            (
                IStringLocalizer<CreateDoctorValidator> stringLocalizer

            )
        {
            _stringLocalizer = stringLocalizer;

            RuleFor(x => x.DoctorId)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"]);

            RuleFor(x => x.Date)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"]);
        }
    }
}