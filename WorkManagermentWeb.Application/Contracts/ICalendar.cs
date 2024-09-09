using WorkManagermentWeb.Application.DTOs;
using WorkManagermentWeb.Application.DTOs.Responses;

namespace WorkManagermentWeb.Application.Contracts
{
    /// <summary>
    /// ICalendar
    /// </summary>
    public interface ICalendar
    {
        /// <summary>
        /// GetEventsByUser
        /// </summary>
        /// <param name="getEventsDTO"></param>
        /// <returns></returns>
        Task<EventsResponse> GetEventsByUser(GetEventsDTO getEventsDTO);

        /// <summary>
        /// SyncEvents
        /// </summary>
        /// <param name="syncEventDTO"></param>
        /// <returns></returns>
        Task<PostPutResponse> SyncEvents(SyncEventsDTO syncEventDTO);

        /// <summary>
        /// SyncEvent
        /// </summary>
        /// <param name="event"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<PostPutResponse> SyncEvent(CalendarEventDTO @event, string token);
    }
}
