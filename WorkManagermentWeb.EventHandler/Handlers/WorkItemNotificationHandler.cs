using MediatR;
using WorkManagermentWeb.EventHandler.Events;
using WorkManagermentWeb.Pusher.Models;
using WorkManagermentWeb.Pusher.Services;

namespace WorkManagermentWeb.EventHandler.Handlers
{
    /// <summary>
    /// WorkItemNotificationHandler
    /// </summary>
    public class WorkItemNotificationHandler :
        INotificationHandler<MediatRMessage<WorkItemCreate>>,
        INotificationHandler<MediatRMessage<WorkItemUpdate>>,
        INotificationHandler<MediatRMessage<BoardMemberAdd>>,
        INotificationHandler<MediatRMessage<BoardMemberRemove>>,
        INotificationHandler<MediatRMessage<WorkSpaceMemberAdd>>,
        INotificationHandler<MediatRMessage<WorkSpaceMemberRemove>>
    {
        /// <summary>
        /// ISendMessageService
        /// </summary>
        private readonly ISendMessageService _sendMessageService;

        /// <summary>
        /// WorkItemNotificationHandler
        /// </summary>
        /// <param name="sendMessageService"></param>
        public WorkItemNotificationHandler(ISendMessageService sendMessageService)
        {
            _sendMessageService = sendMessageService;
        }

        /// <summary>
        /// Handle
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(MediatRMessage<WorkItemCreate> notification, CancellationToken cancellationToken)
        {
            await _sendMessageService.SendAsync(new NotificationInput()
            {
                VietnameseTitle = $"{notification.Message.CreatorName} đã tạo công việc #{notification.Message.WorkItemCode}: {notification.Message.WorkItemTitle}",
                EnglishTitle = $"{notification.Message.CreatorName} has created work item #{notification.Message.WorkItemCode}: {notification.Message.WorkItemTitle}",
                RedirectAt = $"/task/{notification.Message.ObjectId}",
                RecieverId = notification.Message.RecieverId,
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Handle
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Handle(MediatRMessage<WorkItemUpdate> notification, CancellationToken cancellationToken)
        {
            await _sendMessageService.SendAsync(new NotificationInput()
            {
                VietnameseTitle = $"{notification.Message.UpdaterName} đã cập nhật công việc #{notification.Message.WorkItemCode}: {notification.Message.WorkItemTitle}",
                EnglishTitle = $"{notification.Message.UpdaterName} has updated work item #{notification.Message.WorkItemCode}: {notification.Message.WorkItemTitle}",
                RedirectAt = $"/task/{notification.Message.ObjectId}",
                RecieverId = notification.Message.RecieverId,
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Handle
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Handle(MediatRMessage<BoardMemberAdd> notification, CancellationToken cancellationToken)
        {
            await _sendMessageService.SendAsync(new NotificationInput()
            {
                VietnameseTitle = $"{notification.Message.UpdaterName} đã thêm bạn vào dự án {notification.Message.BoardName}",
                EnglishTitle = $"{notification.Message.UpdaterName} has added you to project {notification.Message.BoardName}",
                RedirectAt = $"/project?id={notification.Message.ObjectId}",
                RecieverId = notification.Message.RecieverId,
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Handle
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(MediatRMessage<BoardMemberRemove> notification, CancellationToken cancellationToken)
        {
            await _sendMessageService.SendAsync(new NotificationInput()
            {
                VietnameseTitle = $"{notification.Message.UpdaterName} đã xóa bạn khỏi dự án {notification.Message.BoardName}",
                EnglishTitle = $"{notification.Message.UpdaterName} has removed you out of project {notification.Message.BoardName}",
                RedirectAt = $"/project?id={notification.Message.ObjectId}",
                RecieverId = notification.Message.RecieverId,
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Handle
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Handle(MediatRMessage<WorkSpaceMemberAdd> notification, CancellationToken cancellationToken)
        {
            await _sendMessageService.SendAsync(new NotificationInput()
            {
                VietnameseTitle = $"{notification.Message.UpdaterName} đã thêm bạn vào không gian làm việc {notification.Message.WorkSpaceName}",
                EnglishTitle = $"{notification.Message.UpdaterName} has added you to workspace #{notification.Message.WorkSpaceName}",
                RedirectAt = $"/workspace?id={notification.Message.ObjectId}",
                RecieverId = notification.Message.RecieverId,
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// Handle
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(MediatRMessage<WorkSpaceMemberRemove> notification, CancellationToken cancellationToken)
        {
            await _sendMessageService.SendAsync(new NotificationInput()
            {
                VietnameseTitle = $"{notification.Message.UpdaterName} đã xóa bạn khỏi không gian làm việc {notification.Message.WorkSpaceName}",
                EnglishTitle = $"{notification.Message.UpdaterName} has removed you out of workspace #{notification.Message.WorkSpaceName}",
                RedirectAt = $"/workspace?id={notification.Message.ObjectId}",
                RecieverId = notification.Message.RecieverId,
            }).ConfigureAwait(false);
        }
    }
}
