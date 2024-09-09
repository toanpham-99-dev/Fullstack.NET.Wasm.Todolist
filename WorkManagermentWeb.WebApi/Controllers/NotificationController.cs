using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkManagermentWeb.Application.Constants;
using WorkManagermentWeb.Application.Contracts;
using WorkManagermentWeb.Application.DTOs;
using WorkManagermentWeb.Application.DTOs.Responses;
using WorkManagermentWeb.Application.Utilities;

namespace WorkManagermentWeb.WebApi.Controllers
{
    /// <summary>
    /// NotificationController
    /// </summary>
    [Authorize(AuthorizationConstants.TwoAuthPolicy)]
    [Route($"{ApiRouteConstants.PrefixUrl}/{ApiRouteConstants.Notification}")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        /// <summary>
        /// INotification
        /// </summary>
        private readonly INotification _notification;

        /// <summary>
        /// NotificationController
        /// </summary>
        /// <param name="notification"></param>
        public NotificationController(INotification notification)
        {
            _notification = notification;
        }

        /// <summary>
        /// GetList
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<GetListNotiResponse>> GetList([FromQuery] int skip, [FromQuery] int take, [FromHeader] string language)
        {
            if (skip < 0 || take < 0 || string.IsNullOrEmpty(language))
            {
                return BadRequest();
            }
            string? userId = Helper.GetCurrentUserId(this.User);

            if (userId is null)
            {
                return Unauthorized();
            }
            GetListNotiResponse response = await _notification.GetListAsync(new GetListNotiDTO
            {
                UserId = userId,
                Language = language,
                Paging = new PagingDTO
                {
                    Skip = skip,
                    Take = take
                }
            });
            return Ok(response);
        }

        /// <summary>
        /// MarkAllAsRead
        /// </summary>
        /// <returns></returns>
        [HttpPut(ApiRouteConstants.MarkAsRead)]
        public async Task<ActionResult<MarkAllAsReadResponse>> MarkAllAsRead()
        {
            string? userId = Helper.GetCurrentUserId(this.User);
            if (userId is null)
            {
                return Unauthorized();
            }

            MarkAllAsReadResponse response = await _notification.MarkAllAsReadAsync(userId);
            if (!response.Flag)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="action"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut($"{{{ApiRouteConstants.Id}}}")]
        public async Task<ActionResult<MarkAsReadResponse>> Update([FromQuery] string action, string id)
        {
            Guid notificationId;
            if (!Guid.TryParse(id, out notificationId))
            {
                return BadRequest();
            }

            if (String.Compare(action, HttpConstants.MarkAsReadAction) == 0)
            {
                MarkAsReadResponse response = await _notification.MarkAsReadAsync(notificationId);
                if (!response.Flag)
                {
                    return NotFound(response);
                }
                return Ok(response);
            }
            return BadRequest();
        }
    }
}
