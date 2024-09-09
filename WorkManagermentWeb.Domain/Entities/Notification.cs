using System.ComponentModel.DataAnnotations;
using WorkManagermentWeb.Domain.Entities.Base;
using WorkManagermentWeb.Pusher.IEntities;
#nullable disable

namespace WorkManagermentWeb.Domain.Entities
{
    /// <summary>
    /// Notification
    /// </summary>
    public class Notification : BaseModel, INotification
    {
        /// <summary>
        /// RedirectAt
        /// </summary>
        public string RedirectAt { get; set; }

        /// <summary>
        /// IsRead
        /// </summary>
        [Required]
        public bool IsRead { get; set; } = false;

        /// <summary>
        /// RecieverId
        /// </summary>
        public string RecieverId { get; set; }

        /// <summary>
        /// Reciever
        /// </summary>
        public virtual ApplicationUser Reciever { get; set; }

        /// <summary>
        /// Languages
        /// </summary>
        public virtual ICollection<NotificationLanguage> Languages { get; set; }
    }
}
