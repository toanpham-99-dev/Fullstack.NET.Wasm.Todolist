namespace WorkManagermentWeb.Application.DTOs.Responses
{
    /// <summary>
    /// GetMSEventsResponse
    /// </summary>
    public record GetMSEventsResponse(bool Flag, List<CalendarEventDTO> Events);
}
