using WorkManagermentWeb.Pusher.Models;

namespace WorkManagermentWeb.Pusher.Services
{
    /// <summary>
    /// ISendMessageService
    /// </summary>
    public interface ISendMessageService
    {
        /// <summary>
        /// Send
        /// </summary>
        /// <param name="notificationInput"></param>
        /// <returns></returns>
        Task<bool> SendAsync(NotificationInput notificationInput);
    }
}
