namespace WorkManagermentWeb.Pusher.IEntities
{
    public interface INotification
    {
        /// <summary>
        /// Id
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// RedirectAt
        /// </summary>
        string RedirectAt { get; set; }

        /// <summary>
        /// CreatedAt
        /// </summary>
        DateTime CreatedAt { get; set; }

        /// <summary>
        /// IsRead
        /// </summary>
        bool IsRead { get; set; }

        /// <summary>
        /// RecieverId
        /// </summary>
        string RecieverId { get; set; }

    }
}
