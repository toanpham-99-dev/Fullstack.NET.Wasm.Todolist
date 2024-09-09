namespace WorkManagermentWeb.Application.DTOs
{
    /// <summary>
    /// SyncEventDTO
    /// </summary>
    public class SyncEventDTO
    {
        /// <summary>
        /// CalendarEventDTO
        /// </summary>
        public CalendarEventDTO CalendarEventDTO { get; set; } = new CalendarEventDTO();

        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; } = string.Empty;
    }
}
