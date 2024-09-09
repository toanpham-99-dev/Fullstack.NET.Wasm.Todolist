using WorkManagermentWeb.Application.DTOs.Responses;
using WorkManagermentWeb.Application.DTOs;

namespace WorkManagermentWeb.Application.Contracts
{
    public interface IBoard
    {
        /// <summary>
        /// AddAsync
        /// </summary>
        /// <param name="boardDTO"></param>
        /// <returns></returns>
        Task<PostPutResponse> AddAsync(PostPutBoardDTO boardDTO);

        /// <summary>
        /// UpdateAsync
        /// </summary>
        /// <param name="boardDTO"></param>
        /// <returns></returns>
        Task<PostPutResponse> UpdateAsync(PostPutBoardDTO boardDTO);

        /// <summary>
        /// GetListAsync
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<GetBoardsResponse> GetListAsync(GetBoardsDTO filter);

        /// <summary>
        /// GetByIdAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<GetBoardResponse> GetByIdAsync(Guid id);

        /// <summary>
        /// AddMemberAsync
        /// </summary>
        /// <param name="memberDTO"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PostPutResponse> AddMemberAsync(BoardMemberDTO memberDTO, Guid id);

        /// <summary>
        /// RemoveMemberAsync
        /// </summary>
        /// <param name="memberDTO"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PostPutResponse> RemoveMemberAsync(BoardMemberDTO memberDTO, Guid id);
    }
}
