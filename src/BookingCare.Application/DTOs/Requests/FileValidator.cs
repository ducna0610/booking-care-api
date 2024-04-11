using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace BookingCare.Application.DTOs.Requests
{
    public class FileValidator : AbstractValidator<IFormFile>
    {
        private readonly IStringLocalizer<FileValidator> _stringLocalizer;
        public FileValidator(IStringLocalizer<FileValidator> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;

            RuleFor(x => x.Length).LessThanOrEqualTo(500_000)
                .WithMessage(_stringLocalizer["LessThanValidator"] + "byte")
                .WithName(_stringLocalizer["File"]);
        }
    }
}