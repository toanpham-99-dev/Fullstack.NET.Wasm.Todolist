namespace WorkManagermentWeb.Pusher.Models
{
    /// <summary>
    /// NotificationOutput
    /// </summary>
    public class NotificationOutput
    {
        /// <summary>
        /// NotificationInput
        /// </summary>
        public NotificationInput NotificationInput { get; set; }

        /// <summary>
        /// CreatedDate
        /// </summary>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// IsRead
        /// </summary>
        public bool IsRead { get; set; } = false;

        /// <summary>
        /// NotificationOutput
        /// </summary>
        /// <param name="notificationInput"></param>
        public NotificationOutput(NotificationInput notificationInput)
        {
            NotificationInput = notificationInput;
        }
    }
}
