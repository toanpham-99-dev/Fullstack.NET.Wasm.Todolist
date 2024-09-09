using WorkManagermentWeb.Application.DTOs;
using WorkManagermentWeb.Infrastructure.Models;

namespace WorkManagermentWeb.Infrastructure.Services
{
    /// <summary>
    /// MicrosoftCalendarService
    /// </summary>
    public interface IMicrosoftCalendarService
    {
        /// <summary>
        /// AddEventAsync
        /// </summary>
        /// <param name="calendarEvent"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<GraphEventResponse> AddEventAsync(CalendarEventDTO calendarEvent, string token);

        /// <summary>
        /// DeleteEventAsync
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<GraphEventResponse> DeleteEventAsync(string eventId, string token);
    }
}
