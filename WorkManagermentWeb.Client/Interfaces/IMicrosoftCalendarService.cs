using WorkManagermentWeb.Application.DTOs;
using WorkManagermentWeb.Application.DTOs.Responses;

namespace WorkManagermentWeb.Client.Interfaces
{
    /// <summary>
    /// MicrosoftCalendarService
    /// </summary>
    public interface IMicrosoftCalendarService
    {
        /// <summary>
        /// CheckExternalAuthState
        /// </summary>
        /// <returns></returns>
        Task<bool> CheckExternalAuthState();

        /// <summary>
        /// GetEvents
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        Task<GetMSEventsResponse> GetEvents(int? year, int? month);

        /// <summary>
        /// AddEventAsync
        /// </summary>
        /// <param name="calendarEvent"></param>
        /// <returns></returns>
        Task AddEventAsync(CalendarEventDTO calendarEvent);

        /// <summary>
        /// SyncEvents
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task SyncEvents(string userId);
    }
}
