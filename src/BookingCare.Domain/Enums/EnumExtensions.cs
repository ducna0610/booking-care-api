using BookingCare.Domain.Common;

namespace BookingCare.Domain.Enums
{
    #region DescriptionAttribute
    public class ViDescriptionAttribute : Attribute
    {
        public ViDescriptionAttribute(string stringValue)
        {
            this.stringValue = stringValue;
        }
        private string stringValue;
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
    public class EnDescriptionAttribute : Attribute
    {
        public EnDescriptionAttribute(string stringValue)
        {
            this.stringValue = stringValue;
        }
        private string stringValue;
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
    #endregion

    public static class EnumExtensions
    {
        static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            return (T)attributes[0];
        }

        public static string GetDescription(this Enum value)
        {
            switch (Thread.CurrentThread.CurrentCulture.Name)
            {
                case LanguageCode.VietNamese:
                    return GetViDescription(value);
                default:
                    return GetEnDescription(value);
            }
        }

        public static string GetViDescription(this Enum value)
        {
            var attribute = value.GetAttribute<ViDescriptionAttribute>();
            return attribute == null ? value.ToString() : attribute.StringValue;
        }

        public static string GetEnDescription(this Enum value)
        {
            var attribute = value.GetAttribute<EnDescriptionAttribute>();
            return attribute == null ? value.ToString() : attribute.StringValue;
        }

        public static IEnumerable<string> GetAllDescriptions<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<Enum>().Select(x => x.GetDescription());
        }

        public static Dictionary<int, string> GetDictionaryDescriptions<T>()
        {
            return Enum.GetValues(typeof(T))
                        .Cast<Enum>()
                        .ToDictionary(t => (int)(object)t, t => t.GetDescription());
        }

        public static List<KeyValuePair<int, string>> GetListDescriptions<T>()
        {
            return Enum.GetValues(typeof(T))
                       .Cast<Enum>()
                       .Select(e => new KeyValuePair<int, string>((int)(object)e, e.GetDescription()))
                       .ToList();
        }
    }
}
