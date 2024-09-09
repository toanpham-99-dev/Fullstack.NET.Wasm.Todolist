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
    /// BoardController
    /// </summary>
    [Authorize(AuthorizationConstants.TwoAuthPolicy)]
    [Route($"{ApiRouteConstants.PrefixUrl}/{ApiRouteConstants.Board}")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        /// <summary>
        /// IBoard
        /// </summary>
        private readonly IBoard _board;

        /// <summary>
        /// BoardController
        /// </summary>
        /// <param name="board"></param>
        public BoardController(IBoard board)
        {
            _board = board;
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="boardDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = $"{UserRoleConstants.CEO},{UserRoleConstants.ProjectManager}")]
        public async Task<ActionResult<PostPutResponse>> Add(PostPutBoardDTO boardDTO)
        {
            if (boardDTO == null || boardDTO.WorkSpaceId == Guid.Empty)
            {
                return BadRequest();
            }
            string? userId = Helper.GetCurrentUserId(this.User);

            if (userId is null)
            {
                return Unauthorized();
            }
            boardDTO.LastUpdateBy = userId;
            PostPutResponse response = await _board.AddAsync(boardDTO);
            if (!response.Flag)
                return NotFound(response);
            return Ok(response);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="boardDTO"></param>
        /// <returns></returns>
        [Authorize(Roles = $"{UserRoleConstants.CEO},{UserRoleConstants.ProjectManager}")]
        [HttpPut]
        public async Task<ActionResult<PostPutResponse>> Update(PostPutBoardDTO boardDTO)
        {
            if (boardDTO == null || boardDTO.WorkSpaceId == Guid.Empty || boardDTO.Id == Guid.Empty)
            {
                return BadRequest();
            }
            string? userId = Helper.GetCurrentUserId(this.User);

            if (userId is null)
            {
                return Unauthorized();
            }
            boardDTO.LastUpdateBy = userId;
            PostPutResponse response = await _board.UpdateAsync(boardDTO);
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
        public async Task<ActionResult<GetBoardsResponse>> GetList()
        {
            string? userId = Helper.GetCurrentUserId(this.User);

            if (userId is null)
            {
                return Unauthorized();
            }

            GetBoardsResponse response = await _board.GetListAsync(new GetBoardsDTO
            {
                UserId = userId
            });
            return Ok(response);
        }

        /// <summary>
        /// GetById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet($"{{{ApiRouteConstants.Id}}}")]
        public async Task<ActionResult<GetBoardResponse>> GetById(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return BadRequest();
            }
            Guid boardId;
            if (Guid.TryParse(id, out boardId))
            {
                var result = await _board.GetByIdAsync(boardId);
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
        /// <param name="id"></param>
        /// <param name="memberDTO"></param>
        /// <returns></returns>
        [Authorize(Roles = $"{UserRoleConstants.CEO},{UserRoleConstants.ProjectManager}")]
        [HttpPost($"{{{ApiRouteConstants.Id}}}/{ApiRouteConstants.Members}")]
        public async Task<ActionResult<PostPutResponse>> AddMember([FromBody] BoardMemberDTO memberDTO, Guid id)
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
            PostPutResponse response = await _board.AddMemberAsync(memberDTO, id);
            if (!response.Flag)
                return NotFound(response);
            return Ok(response);
        }

        /// <summary>
        /// RemoveMember
        /// </summary>
        /// <param name="id"></param>
        /// <param name="memberDTO"></param>
        /// <returns></returns>
        [Authorize(Roles = $"{UserRoleConstants.CEO},{UserRoleConstants.ProjectManager}")]
        [HttpDelete($"{{{ApiRouteConstants.Id}}}/{ApiRouteConstants.Members}")]
        public async Task<ActionResult<PostPutResponse>> RemoveMember([FromQuery] BoardMemberDTO memberDTO, Guid id)
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
            PostPutResponse response = await _board.RemoveMemberAsync(memberDTO, id);
            if (!response.Flag)
                return NotFound(response);
            return Ok(response);
        }
    }
}
