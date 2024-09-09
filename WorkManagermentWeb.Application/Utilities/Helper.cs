using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Text.RegularExpressions;
using WorkManagermentWeb.Application.Constants;
using WorkManagermentWeb.Application.DTOs;
using WorkManagermentWeb.Domain.Enums;

namespace WorkManagermentWeb.Application.Utilities
{
    /// <summary>
    /// Helper
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// SEAsiaTimeZoneId
        /// </summary>
        public static string SEAsiaTimeZoneId = "SE Asia Standard Time";

        /// <summary>
        /// ToDateTimeOffset
        /// </summary>
        /// <param name="dateOnly"></param>
        /// <param name="zone"></param>
        /// <returns></returns>
        public static DateTimeOffset ToDateTimeOffset(this DateOnly dateOnly, TimeZoneInfo zone)
        {
            var dateTime = dateOnly.ToDateTime(new TimeOnly(0));
            return new DateTimeOffset(dateTime, zone.GetUtcOffset(dateTime));
        }

        /// <summary>
        /// ToDateOnly
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="zone"></param>
        /// <returns></returns>
        public static DateOnly ToDateOnly(this DateTimeOffset dto, TimeZoneInfo zone)
        {
            var inTargetZone = TimeZoneInfo.ConvertTime(dto, zone);

            return DateOnly.FromDateTime(inTargetZone.Date);
        }

        /// <summary>
        /// ToDateTime
        /// </summary>
        /// <param name="dateOnly"></param>
        /// <param name="timeOnly"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(DateOnly dateOnly, TimeOnly timeOnly)
        {
            var referenceDate = new DateTime(dateOnly.Year, dateOnly.Month, dateOnly.Day);
            referenceDate += timeOnly.ToTimeSpan();
            return referenceDate;
        }

        /// <summary>
        /// GetCurrentUserId
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        /// <returns></returns>
        public static string? GetCurrentUserId(ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        /// <summary>
        /// FormatProfileHtml
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string FormatProfileHtml(UserDTO? user)
        {
            if (user is not null)
            {
                return string.Format(EmailAndHtmlPatterms.UserInfo,
                    user.FullName, user.Email, user.Role.DisplayName);
            }
            return string.Empty;
        }

        /// <summary>
        /// ConvertToTimeZonePlus7
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ConvertToTimeZonePlus7(this DateTime date)
        {
            TimeZoneInfo nzTimeZone = GetTimeZonePlus7();
            DateTime result = TimeZoneInfo.ConvertTimeFromUtc(date, nzTimeZone);
            return result;
        }

        /// <summary>
        /// ConvertToUtc
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ConvertToUtc(this DateTime date)
        {
            TimeZoneInfo nzTimeZone = GetTimeZonePlus7();
            DateTime result = TimeZoneInfo.ConvertTimeToUtc(date, nzTimeZone);
            return result;
        }

        /// <summary>
        /// GetTimeZonePlus7
        /// </summary>
        /// <returns></returns>
        public static TimeZoneInfo GetTimeZonePlus7()
        {
            string timeZoneId = SEAsiaTimeZoneId;
            return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        }

        /// <summary>
        /// CalculateProcessPercent
        /// </summary>
        /// <param name="start"></param>
        /// <param name="due"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static double CalculateProcessPercent(DateOnly? start, DateOnly? due, WorkItemStatus status)
        {
            if (start is null || due is null)
            {
                return 0;
            }
            if (status == WorkItemStatus.Resolved || status == WorkItemStatus.Done)
            {
                return 1;
            }
            DateTime startDate = Helper.ToDateTime(start.Value, new TimeOnly(1, 0, 0));
            DateTime dueDate = Helper.ToDateTime(due.Value, new TimeOnly(10, 59, 59));
            DateTime utcToDay = DateTime.UtcNow;
            if (DateTime.Compare(utcToDay, dueDate) > 0)
            {
                return -1;
            }
            double result = ((utcToDay - startDate).TotalMinutes) / ((dueDate - startDate).TotalMinutes);
            return result < 0 ? 0 : result;
        }

        /// <summary>
        /// GetEnumMemberValue
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumMemberValue(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            var attributes = field!.GetCustomAttributes(typeof(EnumMemberAttribute), false);

            return ((EnumMemberAttribute)attributes[0]).Value!;
        }

        /// <summary>
        /// GetConstFieldAttributeValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="fieldName"></param>
        /// <param name="valueSelector"></param>
        /// <returns></returns>
        public static TOut GetConstFieldAttributeValue<T, TOut, TAttribute>(
            string fieldName,
            Func<TAttribute, TOut> valueSelector)
            where TAttribute : Attribute
        {
            var fieldInfo = typeof(T).GetField(fieldName, BindingFlags.Public | BindingFlags.Static);
            if (fieldInfo == null)
            {
                return default(TOut)!;
            }
            var att = fieldInfo.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
            return att != null ? valueSelector(att) : default(TOut)!;
        }

        /// <summary>
        /// SelectAsync
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<TResult>> SelectAsync<TSource, TResult>(
            this IEnumerable<TSource> source, Func<TSource, Task<TResult>> method,
            int concurrency = int.MaxValue)
        {
            var semaphore = new SemaphoreSlim(concurrency);
            try
            {
                return await Task.WhenAll(source.Select(async s =>
                {
                    try
                    {
                        await semaphore.WaitAsync();
                        return await method(s);
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }));
            }
            finally
            {
                semaphore.Dispose();
            }
        }

        /// <summary>
        /// Base64Encode
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// IsInputMatchRegex
        /// </summary>
        /// <param name="input"></param>
        /// <param name="regex"></param>
        /// <returns></returns>
        public static bool IsInputMatchRegex(this string input, string regex)
        {
            var match = Regex.Match(input, regex);
            return match.Success;
        }

        /// <summary>
        /// SetCulture
        /// </summary>
        /// <param name="culture"></param>
        public static void SetCulture(string culture)
        {
            CultureInfo cultureInfo = new CultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }
    }
}
