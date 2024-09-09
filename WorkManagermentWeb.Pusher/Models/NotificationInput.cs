namespace WorkManagermentWeb.Pusher.Models
{
    public class NotificationInput
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// VietnamseTitle
        /// </summary>
        public string VietnameseTitle { get; set; } = string.Empty;

        /// <summary>
        /// EnglishTitle
        /// </summary>
        public string EnglishTitle { get; set; } = string.Empty;

        /// <summary>
        /// RedirectAt
        /// </summary>
        public string RedirectAt { get; set; } = string.Empty;

        /// <summary>
        /// RecieverId
        /// </summary>
        public string RecieverId { get; set; } = string.Empty;
    }
}
