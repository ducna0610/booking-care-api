using System.ComponentModel;

namespace BookingCare.API.Controllers.Common.Wrapper
{
    public class SuccessResult : IResult
    {
        public string Message { get; set; } = string.Empty;

        [DefaultValue(true)]
        public bool Succeeded { get; set; } = true;

        public SuccessResult()
        {
        }

        public SuccessResult(string message)
        {
            Message = message;
        }
    }

    public class SuccessResult<T> : SuccessResult, IResult<T>
    {
        public T Data { get; set; }

        public SuccessResult()
        {
        }

        public SuccessResult(T data)
        {
            Data = data;
        }

        public SuccessResult(T data, string message)
        {
            Data = data;
            Message = message;
        }
    }
}