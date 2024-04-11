using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookingCare.Application.DTOs.Requests.Doctor
{
    public class SetScheduleValidator : AbstractValidator<SetScheduleRequest>
    {
        private readonly IStringLocalizer<CreateDoctorValidator> _stringLocalizer;
        public SetScheduleValidator
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
                .WithMessage(_stringLocalizer["NotNullValidator"])
                .GreaterThan(DateTimeOffset.Now)
                .WithMessage(string.Format(_stringLocalizer["DateGreaterThanValidator"], DateTime.Now.ToString("dd/MM/yyyy")));

            RuleForEach(x => x.TimeSelects)
                .IsInEnum()
                .WithMessage(_stringLocalizer["NotValidValidator"])
                .WithName(_stringLocalizer["TimeSelects"]);
        }
    }
}