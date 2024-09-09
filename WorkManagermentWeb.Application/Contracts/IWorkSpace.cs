using WorkManagermentWeb.Application.DTOs.Responses;
using WorkManagermentWeb.Application.DTOs;

namespace WorkManagermentWeb.Application.Contracts
{
    public interface IWorkSpace
    {
        /// <summary>
        /// AddAsync
        /// </summary>
        /// <param name="workSpaceDTO"></param>
        /// <returns></returns>
        Task<PostPutResponse> AddAsync(WorkSpaceDTO workSpaceDTO);

        /// <summary>
        /// UpdateAsync
        /// </summary>
        /// <param name="workSpaceDTO"></param>
        /// <returns></returns>
        Task<PostPutResponse> UpdateAsync(WorkSpaceDTO workSpaceDTO);

        /// <summary>
        /// GetListAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<GetWorkspacesResponse> GetListAsync(string userId);

        /// <summary>
        /// GetByIdAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<GetWorkspaceResponse> GetByIdAsync(Guid id);

        /// <summary>
        /// AddMemberAsync
        /// </summary>
        /// <returns></returns>
        Task<PostPutResponse> AddMemberAsync(Guid workSpaceId, string memberId, string updaterId);

        /// <summary>
        /// RemoveMemberAsync
        /// </summary>
        /// <param name="workSpaceId"></param>
        /// <param name="memberId"></param>
        /// <param name="updaterId"></param>
        /// <returns></returns>
        Task<PostPutResponse> RemoveMemberAsync(Guid workSpaceId, string memberId, string updaterId);
    }
}
