namespace WorkManagermentWeb.Pusher.IEntities
{
    /// <summary>
    /// INotificationLanguage
    /// </summary>
    public interface INotificationLanguage
    {
        /// <summary>
        /// Id
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// NotificationId
        /// </summary>
        Guid NotificationId { get; set; }

        /// <summary>
        /// Lang
        /// </summary>
        string Lang { get; set; }

        /// <summary>
        /// Content
        /// </summary>
        string Content { get; set; }
    }
}
