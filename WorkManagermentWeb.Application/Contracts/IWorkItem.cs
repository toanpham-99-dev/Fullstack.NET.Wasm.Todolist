using WorkManagermentWeb.Application.DTOs;
using WorkManagermentWeb.Application.DTOs.Responses;
using WorkManagermentWeb.Domain.Entities;
using WorkManagermentWeb.Domain.Enums;

namespace WorkManagermentWeb.Application.Contracts
{
    /// <summary>
    /// IWorkItem
    /// </summary>
    public interface IWorkItem
    {
        /// <summary>
        /// AddAsync
        /// </summary>
        /// <param name="workItemDTO"></param>
        /// <returns></returns>
        Task<PostWorkItemResponse> AddAsync(PostPutWorkItemDTO workItemDTO);

        /// <summary>
        /// UpdateAsync
        /// </summary>
        /// <param name="workItemDTO"></param>
        /// <returns></returns>
        Task<PostWorkItemResponse> UpdateAsync(PostPutWorkItemDTO workItemDTO);

        /// <summary>
        /// DeleteAsync
        /// </summary>
        /// <param name="workItemId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<PostWorkItemResponse> DeleteAsync(Guid workItemId, string userId);

        /// <summary>
        /// ChangeStatusAsync
        /// </summary>
        /// <param name="workItemId"></param>
        /// <param name="status"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<PostWorkItemResponse> ChangeStatusAsync(Guid workItemId, WorkItemStatus status, string userId);

        /// <summary>
        /// GetListAsync
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<GetWorkItemsResponse> GetListAsync(WorkItemFilterDTO filter);

        /// <summary>
        /// GetByIdAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<GetWorkItemResponse> GetByIdAsync(Guid id);

        /// <summary>
        /// QueryAllNearingDues
        /// </summary>
        /// <returns></returns>
        IQueryable<WorkItem> QueryAllNearingDues();

        /// <summary>
        /// GetSpecicalPropertiesInfo
        /// </summary>
        /// <returns></returns>
        GetPropertiesInfoResponse GetSpecicalPropertiesInfo();

        /// <summary>
        /// MarkAsSyncToCalendar
        /// </summary>
        /// <param name="markAsSyncToCalendarDTO"></param>
        /// <returns></returns>
        Task<PostPutResponse> MarkAsSyncToCalendar(MarkAsSyncToCalendarDTO markAsSyncToCalendarDTO);
    }
}
