using Microsoft.AspNetCore.Mvc;
using WorkManagermentWeb.Application.Contracts;
using WorkManagermentWeb.Application.DTOs.Responses;
using WorkManagermentWeb.Application.DTOs;
using WorkManagermentWeb.Application.Utilities;
using WorkManagermentWeb.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using WorkManagermentWeb.Application.Constants;

namespace WorkManagermentWeb.WebApi.Controllers
{
    /// <summary>
    /// WorkItemController
    /// </summary>
    [Route($"{ApiRouteConstants.PrefixUrl}/{ApiRouteConstants.WorkItem}")]
    [ApiController]
    public class WorkItemController : ControllerBase
    {
        /// <summary>
        /// IUser
        /// </summary>
        private readonly IWorkItem _workItem;

        /// <summary>
        /// WorkItemController
        /// </summary>
        /// <param name="workItem"></param>
        public WorkItemController(IWorkItem workItem)
        {
            _workItem = workItem;
        }

        /// <summary>
        /// GetById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(AuthorizationConstants.TwoAuthPolicy)]
        [HttpGet($"{{{ApiRouteConstants.Id}}}")]
        public async Task<ActionResult<GetWorkItemResponse>> GetById(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return BadRequest();
            }
            Guid workItemId;
            if (Guid.TryParse(id, out workItemId))
            {
                var result = await _workItem.GetByIdAsync(workItemId);
                if (!result.Flag)
                    return NotFound(result);
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="workItemDTO"></param>
        /// <returns></returns>
        [Authorize(AuthorizationConstants.TwoAuthPolicy)]
        [HttpPost]
        public async Task<ActionResult<PostWorkItemResponse>> Add(PostPutWorkItemDTO workItemDTO)
        {
            string? userId = Helper.GetCurrentUserId(this.User);

            if (String.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            if (workItemDTO is null || workItemDTO.BoardId == Guid.Empty)
            {
                return BadRequest();
            }
            workItemDTO.CreatorId = userId;
            workItemDTO.LastUpdaterId = userId;
            var result = await _workItem.AddAsync(workItemDTO);
            if (!result.Flag)
                return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="workItemDTO"></param>
        /// <returns></returns>
        [Authorize(AuthorizationConstants.TwoAuthPolicy)]
        [HttpPut]
        public async Task<ActionResult<PostWorkItemResponse>> Update(PostPutWorkItemDTO workItemDTO)
        {
            string? userId = Helper.GetCurrentUserId(this.User);

            if (String.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            if (workItemDTO is null || workItemDTO.BoardId == Guid.Empty || workItemDTO.Id == Guid.Empty)
            {
                return BadRequest();
            }
            workItemDTO.LastUpdaterId = userId;
            var result = await _workItem.UpdateAsync(workItemDTO);
            if (!result.Flag)
                return NotFound(result);
            return Ok(result);
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(AuthorizationConstants.TwoAuthPolicy)]
        [HttpDelete($"{{{ApiRouteConstants.Id}}}")]
        public async Task<ActionResult<PostWorkItemResponse>> Delete(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return BadRequest();
            }
            string? userId = Helper.GetCurrentUserId(this.User);

            if (String.IsNullOrEmpty(userId))
            {
                return NotFound();
            }
            Guid workItemId;
            if (Guid.TryParse(id, out workItemId))
            {
                var result = await _workItem.DeleteAsync(workItemId, userId);
                if (!result.Flag)
                    return NotFound(result);
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// ChangeStatus
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [Authorize(AuthorizationConstants.TwoAuthPolicy)]
        [HttpPut($"{ApiRouteConstants.ChangeStatus}/{{{ApiRouteConstants.Id}}}")]
        public async Task<ActionResult<PostWorkItemResponse>> ChangeStatus(string id, [FromQuery] WorkItemStatus status)
        {
            string? userId = Helper.GetCurrentUserId(this.User);

            if (String.IsNullOrEmpty(userId) || String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            Guid workItemId;
            if (Guid.TryParse(id, out workItemId))
            {
                var result = await _workItem.ChangeStatusAsync(workItemId, status, userId);
                if (!result.Flag)
                    return NotFound(result);
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// GetList
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [Authorize(AuthorizationConstants.TwoAuthPolicy)]
        [HttpGet]
        public async Task<ActionResult<GetWorkItemsResponse>> GetList([FromQuery] WorkItemFilterDTO filter)
        {
            var result = await _workItem.GetListAsync(filter);
            return Ok(result);
        }

        /// <summary>
        /// GetSpecicalPropertiesInfo
        /// </summary>
        /// <returns></returns>
        [HttpGet(ApiRouteConstants.GetSpecialPropertiesInfo)]
        public ActionResult<GetPropertiesInfoResponse> GetSpecicalPropertiesInfo()
        {
            return Ok(_workItem.GetSpecicalPropertiesInfo());
        }

        /// <summary>
        /// MarkAsSyncToCalendar
        /// </summary>
        /// <param name="markAsSyncToCalendarDTO"></param>
        /// <returns></returns>
        [Authorize(AuthorizationConstants.TwoAuthPolicy)]
        [HttpPut($"{ApiRouteConstants.MarkAsSyncToCalendar}")]
        public async Task<ActionResult<PostWorkItemResponse>> MarkAsSyncToCalendar(MarkAsSyncToCalendarDTO markAsSyncToCalendarDTO)
        {
            var result = await _workItem.MarkAsSyncToCalendar(markAsSyncToCalendarDTO)
                .ConfigureAwait(false);
            if (!result.Flag)
                return NotFound(result);
            return Ok(result);
        }
    }
}
