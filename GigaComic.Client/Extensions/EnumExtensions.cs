using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace GigaComic.Client.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
              .GetMember(enumValue.ToString())
              .First()
              .GetCustomAttribute<DisplayAttribute>()
              ?.GetName()!;
        }

        public static List<T> GetOrderedValues<T>()
            where T : Enum
        {
            return GetValues<T>().OrderBy(e => ((DisplayAttribute)typeof(T)
                .GetMember(e.ToString())[0]
                .GetCustomAttributes(typeof(DisplayAttribute), false)[0]).Order)
                .ToList();
        }

        public static List<T> GetValues<T>()
            where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }
    }
}
