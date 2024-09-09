using WorkManagermentWeb.Pusher.IEntities;
#nullable disable

namespace WorkManagermentWeb.Domain.Entities
{
    /// <summary>
    /// NotificationLanguage
    /// </summary>
    public class NotificationLanguage : INotificationLanguage
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// NotificationId
        /// </summary>
        public Guid NotificationId { get; set; }

        /// <summary>
        /// Lang
        /// </summary>
        public string Lang { get; set; } = string.Empty;

        /// <summary>
        /// Content
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Notification
        /// </summary>
        public virtual Notification Notification { get; set; }
    }
}
