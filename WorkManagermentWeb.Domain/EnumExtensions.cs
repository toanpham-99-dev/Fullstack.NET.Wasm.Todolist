using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace WorkManagermentWeb.Domain
{
    /// <summary>
    /// EnumExtensions
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// GetDisplayDescription
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetDisplayDescription(this Enum enumValue)
        {
            return enumValue.GetType().GetMember(enumValue.ToString())
                .FirstOrDefault()?
                .GetCustomAttribute<DisplayAttribute>()?
                .GetDescription() ?? "unknown";
        }
    }
}
