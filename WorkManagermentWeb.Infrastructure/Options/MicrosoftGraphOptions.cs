namespace WorkManagermentWeb.Infrastructure.Options
{
    /// <summary>
    /// MicrosoftGraphOptions
    /// </summary>
    public class MicrosoftGraphOptions
    {
        /// <summary>
        /// BaseApiUrl
        /// </summary>
        public string BaseApiUrl { get; set; } = string.Empty;

        /// <summary>
        /// GetAllEventsUrl
        /// </summary>
        public string GetAllEventsUrl { get; set; } = string.Empty;

        /// <summary>
        /// GetEventsUrl
        /// </summary>
        public string GetEventsUrl { get; set; } = string.Empty;

        /// <summary>
        /// EventUrl
        /// </summary>
        public string EventUrl { get; set; } = string.Empty;
    }
}
