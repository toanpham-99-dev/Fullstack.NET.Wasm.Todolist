using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkManagermentWeb.Application.Constants;
using WorkManagermentWeb.Application.Contracts;
using WorkManagermentWeb.Application.DTOs.Responses;
using WorkManagermentWeb.Application.DTOs;
using WorkManagermentWeb.Application.Utilities;

namespace WorkManagermentWeb.WebApi.Controllers
{
    /// <summary>
    /// CalendarController
    /// </summary>
    [Route($"{ApiRouteConstants.PrefixUrl}/{ApiRouteConstants.Calendar}")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        /// <summary>
        /// ICalendar
        /// </summary>
        private readonly ICalendar _calendar;

        /// <summary>
        /// CalendarController
        /// </summary>
        /// <param name="calendar"></param>
        public CalendarController(ICalendar calendar)
        {
            _calendar = calendar;
        }

        /// <summary>
        /// GetList
        /// </summary>
        /// <returns></returns>
        [Authorize(AuthorizationConstants.TwoAuthPolicy)]
        [HttpGet($"{ApiRouteConstants.CalendarEvents}")]
        public async Task<ActionResult<GetWorkItemsResponse>> GetList()
        {
            var result = await _calendar.GetEventsByUser(new GetEventsDTO()
            {
                UserId = Helper.GetCurrentUserId(this.User)!
            });
            return Ok(result);
        }

        /// <summary>
        /// SyncEvents
        /// </summary>
        /// <param name="syncEventsDTO"></param>
        /// <returns></returns>
        [Authorize(AuthorizationConstants.TwoAuthPolicy)]
        [HttpPost($"{ApiRouteConstants.CalendarEvents}")]
        public async Task<ActionResult<PostPutResponse>> SyncEvents(SyncEventsDTO syncEventsDTO)
        {
            syncEventsDTO.UserId = Helper.GetCurrentUserId(this.User)!;
            var result = await _calendar.SyncEvents(syncEventsDTO);
            return Ok(result);
        }

        /// <summary>
        /// SyncEvents
        /// </summary>
        /// <param name="syncEventDTO"></param>
        /// <returns></returns>
        [Authorize(AuthorizationConstants.TwoAuthPolicy)]
        [HttpPost($"{ApiRouteConstants.CalendarEvent}")]
        public async Task<ActionResult<GetWorkItemsResponse>> SyncEvent(SyncEventDTO syncEventDTO)
        {
            var result = await _calendar.SyncEvent(syncEventDTO.CalendarEventDTO, syncEventDTO.Token);
            return Ok(result);
        }
    }
}
