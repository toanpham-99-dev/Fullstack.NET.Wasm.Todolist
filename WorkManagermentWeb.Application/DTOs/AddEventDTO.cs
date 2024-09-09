namespace WorkManagermentWeb.Application.DTOs
{
    /// <summary>
    /// AddEventDTO
    /// </summary>
    public class AddEventDTO
    {
        /// <summary>
        /// EventDTO
        /// </summary>
        public CalendarEventDTO EventDTO { get; set; } = new CalendarEventDTO();
    }
}
