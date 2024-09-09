namespace WorkManagermentWeb.Application.DTOs
{
    /// <summary>
    /// CalendarEventsDTO
    /// </summary>
    public class CalendarEventDTO
    {
        /// <summary>
        /// WorkItemId
        /// </summary>
        public Guid WorkItemId { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Start
        /// </summary>
        public string Start { get; set; } = string.Empty;

        /// <summary>
        /// End
        /// </summary>
        public string End { get; set; } = string.Empty;

        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; } = string.Empty;
    }
}
