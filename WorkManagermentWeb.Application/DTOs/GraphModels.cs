using System;

namespace WorkManagermentWeb.Client.Models
{
    /// <summary>
    /// GraphEventsResponse
    /// </summary>
    public class GraphEventsResponse
    {
        /// <summary>
        /// MicrosoftGraphEvent
        /// </summary>
        public MicrosoftGraphEvent[] Value { get; set; } = new MicrosoftGraphEvent[0];
    }

    /// <summary>
    /// MicrosoftGraphEvent
    /// </summary>
    public class MicrosoftGraphEvent
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Subject
        /// </summary>
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// Start
        /// </summary>
        public DateTimeTimeZone Start { get; set; } = new DateTimeTimeZone();

        /// <summary>
        /// DateTimeTimeZone
        /// </summary>
        public DateTimeTimeZone End { get; set; } = new DateTimeTimeZone();
    }

    /// <summary>
    /// DateTimeTimeZone
    /// </summary>
    public class DateTimeTimeZone
    {
        /// <summary>
        /// 
        /// </summary>
        public string DateTime { get; set; } = string.Empty;

        /// <summary>
        /// TimeZone
        /// </summary>
        public string TimeZone { get; set; } = string.Empty;

        /// <summary>
        /// ConvertToLocalDateTime
        /// </summary>
        /// <returns></returns>
        public DateTime ConvertToLocalDateTime()
        {
            var dateTime = System.DateTime.Parse(DateTime);

            TimeZoneInfo timeZone = null!;
            if (TimeZone == "UTC")
                timeZone = TimeZoneInfo.Utc;
            else
                timeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZone);

            return new DateTimeOffset(dateTime, timeZone.BaseUtcOffset).LocalDateTime;
        }
    }
}
