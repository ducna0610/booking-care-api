using BookingCare.Application.Repositories;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookingCare.Application.DTOs.Requests.Booking
{
    public class CreateBookingValidator : AbstractValidator<CreateBookingRequest>
    {
        private readonly IStringLocalizer<CreateBookingValidator> _stringLocalizer;
        private readonly IDoctorInfoRepository _doctorInfoRepository;
        private readonly IScheduleRepository _scheduleRepository;

        public CreateBookingValidator
            (
                IStringLocalizer<CreateBookingValidator> stringLocalizer,
                IDoctorInfoRepository doctorInfoRepository,
                IScheduleRepository scheduleRepository
            )
        {
            _stringLocalizer = stringLocalizer;
            _doctorInfoRepository = doctorInfoRepository;
            _scheduleRepository = scheduleRepository;

            RuleFor(x => x.DoctorId)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"]);
            When(x => x.DoctorId != null, () =>
            {
                RuleFor(x => x.DoctorId)
                    .Must((id) =>
                    {
                        if (_doctorInfoRepository.GetById(id) == null)
                            return false;
                        return true;
                    }).WithMessage(_stringLocalizer["NotExistValidator"])
                    .WithName(_stringLocalizer["Doctor"]);
            });


            RuleFor(x => x.ScheduleId)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"]);
            When(x => x.ScheduleId != null, () =>
            {
                RuleFor(x => x.ScheduleId)
                    .Must((id) =>
                    {
                        if (_scheduleRepository.GetById(id) == null)
                            return false;
                        return true;
                    }).WithMessage(_stringLocalizer["NotExistValidator"])
                    .WithName(_stringLocalizer["Schedule"]);
            });

            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage(_stringLocalizer["NotNullValidator"])
                .MaximumLength(50)
                .WithMessage(_stringLocalizer["MaximumLengthValidator"])
                .WithName(_stringLocalizer["UserName"]);

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^0\d{9}$")
                .WithMessage(_stringLocalizer["RegularExpressionValidator"])
                .WithName(_stringLocalizer["PhoneNumber"]);

            RuleFor(x => x.Gender)
                .IsInEnum()
                .WithMessage(_stringLocalizer["NotValidValidator"])
                .WithName(_stringLocalizer["Gender"]);
        }
    }
}