namespace WorkManagermentWeb.Application.DTOs
{
    /// <summary>
    /// NotificationDTO
    /// </summary>
    public class NotificationDTO
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// RedirectAt
        /// </summary>
        public string RedirectAt { get; set; } = string.Empty;

        /// <summary>
        /// CraetedAt
        /// </summary>
        public DateTime CraetedAt { get; set; }

        /// <summary>
        /// IsRead
        /// </summary>
        public bool IsRead { get; set; }
    }
}
