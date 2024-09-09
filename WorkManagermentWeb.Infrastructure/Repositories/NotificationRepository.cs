using Microsoft.EntityFrameworkCore;
using WorkManagermentWeb.Application.Constants;
using WorkManagermentWeb.Application.Contracts;
using WorkManagermentWeb.Application.DTOs;
using WorkManagermentWeb.Application.DTOs.Responses;
using WorkManagermentWeb.Application.Utilities;
using WorkManagermentWeb.Domain.Entities;
using WorkManagermentWeb.Infrastructure.Data;

namespace WorkManagermentWeb.Infrastructure.Repositories
{
    /// <summary>
    /// NotificationRepository
    /// </summary>
    /// <param name="context"></param>
    public class NotificationRepository(AppDBContext context) : INotification
    {
        /// <summary>
        /// GetListAsync
        /// </summary>
        /// <param name="getListNotiDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<GetListNotiResponse> GetListAsync(GetListNotiDTO getListNotiDTO)
        {
            IQueryable<Notification> query = context.Notifications
                .Include(_ => _.Languages)
                .Where(_ => _.RecieverId == getListNotiDTO.UserId);

            int total = await query.CountAsync();

            List<NotificationDTO> notifications = await query.OrderByDescending(_ => _.CreatedAt)
                .Skip(getListNotiDTO.Paging.Skip)
                .Take(getListNotiDTO.Paging.Take)
                .Select(_ => new NotificationDTO
                {
                    Id = _.Id,
                    Title = _.Languages.FirstOrDefault(_ => _.Lang == getListNotiDTO.Language)!.Content,
                    RedirectAt = _.RedirectAt,
                    CraetedAt = _.CreatedAt.ConvertToTimeZonePlus7(),
                    IsRead = _.IsRead
                }).ToListAsync();
            return new GetListNotiResponse(total, notifications);
        }

        /// <summary>
        /// MarkAllAsReadAsync
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MarkAllAsReadResponse> MarkAllAsReadAsync(string userId)
        {
            try
            {
                List<Notification> notifications = await context.Notifications
                    .Where(_ => _.RecieverId == userId && !_.IsRead).ToListAsync();

                notifications.ForEach(_ => _.IsRead = true);

                context.Notifications.UpdateRange(notifications);
                await context.SaveChangesAsync();

                return new MarkAllAsReadResponse(true);
            }
            catch (Exception)
            {
                return new MarkAllAsReadResponse(false);
            }
        }

        /// <summary>
        /// MarkAsReadAsync
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MarkAsReadResponse> MarkAsReadAsync(Guid notificationId)
        {
            Notification? notification = await context.Notifications
                .FirstOrDefaultAsync(_ => _.Id == notificationId);
            if (notification is null)
            {
                return new MarkAsReadResponse(false, HttpConstants.NotificationNotFound);
            }
            if (!notification.IsRead)
            {
                notification.IsRead = true;
                context.Notifications.Update(notification);
                await context.SaveChangesAsync();
            }
            return new MarkAsReadResponse(true);
        }
    }
}
