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
    /// WorkSpaceController
    /// </summary>
    [Authorize(AuthorizationConstants.TwoAuthPolicy)]
    [Route($"{ApiRouteConstants.PrefixUrl}/{ApiRouteConstants.WorkSpace}")]
    [ApiController]
    public class WorkSpaceController : ControllerBase
    {
        /// <summary>
        /// IWorkSpace
        /// </summary>
        private readonly IWorkSpace _workSpace;

        /// <summary>
        /// WorkSpaceController
        /// </summary>
        /// <param name="workSpace"></param>
        public WorkSpaceController(IWorkSpace workSpace)
        {
            _workSpace = workSpace;
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="workSpaceDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = $"{UserRoleConstants.CEO}")]
        public async Task<ActionResult<PostPutResponse>> Add(WorkSpaceDTO workSpaceDTO)
        {
            if (workSpaceDTO == null)
            {
                return BadRequest();
            }
            string? userId = Helper.GetCurrentUserId(this.User);

            if (userId is null)
            {
                return Unauthorized();
            }
            workSpaceDTO.LastUpdateBy = userId;
            PostPutResponse response = await _workSpace.AddAsync(workSpaceDTO);
            if (!response.Flag)
                return NotFound(response);
            return Ok(response);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="workSpaceDTO"></param>
        /// <returns></returns>
        [Authorize(Roles = $"{UserRoleConstants.CEO}")]
        [HttpPut]
        public async Task<ActionResult<PostPutResponse>> Update(WorkSpaceDTO workSpaceDTO)
        {
            if (workSpaceDTO == null || workSpaceDTO.Id == Guid.Empty)
            {
                return BadRequest();
            }
            string? userId = Helper.GetCurrentUserId(this.User);

            if (userId is null)
            {
                return Unauthorized();
            }
            workSpaceDTO.LastUpdateBy = userId;
            PostPutResponse response = await _workSpace.UpdateAsync(workSpaceDTO);
            if (!response.Flag)
                return NotFound(response);
            return Ok(response);
        }

        /// <summary>
        /// GetList
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<GetWorkspacesResponse>> GetList()
        {
            string? userId = Helper.GetCurrentUserId(this.User);

            if (userId is null)
            {
                return Unauthorized();
            }

            GetWorkspacesResponse response = await _workSpace.GetListAsync(userId);
            return Ok(response);
        }

        /// <summary>
        /// GetById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet($"{{{ApiRouteConstants.Id}}}")]
        public async Task<ActionResult<GetWorkspaceResponse>> GetById(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return BadRequest();
            }
            Guid boardId;
            if (Guid.TryParse(id, out boardId))
            {
                var result = await _workSpace.GetByIdAsync(boardId);
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
        /// AddMember
        /// </summary>
        /// <param name="memberDTO"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = $"{UserRoleConstants.CEO}")]
        [HttpPost($"{{{ApiRouteConstants.Id}}}/{ApiRouteConstants.Members}")]
        public async Task<ActionResult<PostPutResponse>> AddMember(WorkSpaceMemberDTO memberDTO, Guid id)
        {
            if (memberDTO == null || id == Guid.Empty || string.IsNullOrEmpty(memberDTO.MemberId))
            {
                return BadRequest();
            }
            string? userId = Helper.GetCurrentUserId(this.User);

            if (userId is null)
            {
                return Unauthorized();
            }
            memberDTO.UpdaterId = userId;
            PostPutResponse response = await _workSpace.AddMemberAsync(id, memberDTO.MemberId, memberDTO.UpdaterId);
            if (!response.Flag)
                return NotFound(response);
            return Ok(response);
        }

        /// <summary>
        /// RemoveMember
        /// </summary>
        /// <param name="memberDTO"></param>
        /// <returns></returns>
        [Authorize(Roles = $"{UserRoleConstants.CEO}")]
        [HttpDelete($"{{{ApiRouteConstants.Id}}}/{ApiRouteConstants.Members}")]
        public async Task<ActionResult<PostPutResponse>> RemoveMember(WorkSpaceMemberDTO memberDTO, Guid id)
        {
            if (memberDTO == null || id == Guid.Empty || string.IsNullOrEmpty(memberDTO.MemberId))
            {
                return BadRequest();
            }
            string? userId = Helper.GetCurrentUserId(this.User);

            if (userId is null)
            {
                return Unauthorized();
            }
            memberDTO.UpdaterId = userId;
            PostPutResponse response = await _workSpace.RemoveMemberAsync(id, memberDTO.MemberId, memberDTO.UpdaterId);
            if (!response.Flag)
                return NotFound(response);
            return Ok(response);
        }

    }
}
