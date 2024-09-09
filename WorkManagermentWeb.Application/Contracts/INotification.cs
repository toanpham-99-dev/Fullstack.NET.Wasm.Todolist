using WorkManagermentWeb.Application.DTOs;
using WorkManagermentWeb.Application.DTOs.Responses;

namespace WorkManagermentWeb.Application.Contracts
{
    /// <summary>
    /// INotification
    /// </summary>
    public interface INotification
    {
        /// <summary>
        /// GetListAsync
        /// </summary>
        /// <param name="getListNotiDTO"></param>
        /// <returns></returns>
        Task<GetListNotiResponse> GetListAsync(GetListNotiDTO getListNotiDTO);

        /// <summary>
        /// MarkAllAsReadAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<MarkAllAsReadResponse> MarkAllAsReadAsync(string userId);

        /// <summary>
        /// MarkAsReadAsync
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        Task<MarkAsReadResponse> MarkAsReadAsync(Guid notificationId);
    }
}
