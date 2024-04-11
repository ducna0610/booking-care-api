using System.ComponentModel;

namespace BookingCare.API.Controllers.Common.Wrapper
{
    public class ErrorResult : IResult
    {
        public string Message { get; set; } = string.Empty;
        public Dictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();


        [DefaultValue(false)]
        public bool Succeeded { get; set; } = false;

        public ErrorResult(Dictionary<string, string[]> errors, string message)
        {
            Errors = errors;
            Message = message;
        }

        public ErrorResult(string message)
        {
            Message = message;
        }
    }
}
