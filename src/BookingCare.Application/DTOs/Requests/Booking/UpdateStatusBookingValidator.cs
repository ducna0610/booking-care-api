using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookingCare.Application.DTOs.Requests.Booking
{
    public class UpdateStatusBookingValidator : AbstractValidator<UpdateStatusBookingRequest>
    {
        private readonly IStringLocalizer<CreateBookingValidator> _stringLocalizer;

        public UpdateStatusBookingValidator
            (
                IStringLocalizer<CreateBookingValidator> stringLocalizer
            )
        {
            _stringLocalizer = stringLocalizer;

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage(_stringLocalizer["NotValidValidator"])
                .WithName(_stringLocalizer["Status"]);
        }
    }
}