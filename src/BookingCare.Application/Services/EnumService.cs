using BookingCare.Domain.Enums;

namespace BookingCare.Application.Services
{
    public interface IEnumService
    {
        IEnumerable<KeyValuePair<int, string>> GetGenderEnum();
        IEnumerable<KeyValuePair<int, string>> GetTimeSelectEnum();
        IEnumerable<KeyValuePair<int, string>> GetStatusEnum();
    }

    public class EnumService : IEnumService
    {
        public IEnumerable<KeyValuePair<int, string>> GetGenderEnum()
        {
            return EnumExtensions.GetListDescriptions<GenderEnum>();
        }

        public IEnumerable<KeyValuePair<int, string>> GetTimeSelectEnum()
        {
            return EnumExtensions.GetListDescriptions<TimeSelectEnum>();
        }

        public IEnumerable<KeyValuePair<int, string>> GetStatusEnum()
        {
            return EnumExtensions.GetListDescriptions<StatusEnum>();
        }
    }
}
