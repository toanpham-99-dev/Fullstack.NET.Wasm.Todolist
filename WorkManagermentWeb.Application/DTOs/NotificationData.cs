using System.Data;
using WorkManagermentWeb.Pusher.Models;

namespace WorkManagermentWeb.Application.DTOs
{
    /// <summary>
    /// NotificationData
    /// </summary>
    public class NotificationData
    {
        /// <summary>
        /// NotificationInput
        /// </summary>
        public NotificationInput NotificationInput { get; set; } = new NotificationInput();

        /// <summary>
        /// CreatedDate
        /// </summary>
        public DataSetDateTime CreatedDate { get; set; }

        /// <summary>
        /// IsRead
        /// </summary>
        public bool IsRead { get; set; }
    }
}
