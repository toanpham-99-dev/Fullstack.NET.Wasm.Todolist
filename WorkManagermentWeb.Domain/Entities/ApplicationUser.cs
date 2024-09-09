using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using WorkManagermentWeb.Domain.Enums;
#nullable disable
namespace WorkManagermentWeb.Domain.Entities
{
    /// <summary>
    /// ApplicationUser
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// FullName
        /// </summary>
        [StringLength(50)]
        public string FullName { get; set; }

        /// <summary>
        /// ActiveStatus
        /// </summary>
        [Required]
        public bool ActiveStatus { get; set; } = true;

        /// <summary>
        /// AccountType
        /// </summary>
        public AccountType AccountType { get; set; }

        /// <summary>
        /// ExternalAccountConnected
        /// </summary>
        [Required]
        public bool ExternalAccountConnected { get; set; }

        /// <summary>
        /// WorkSpaceUsers
        /// </summary>
        public virtual ICollection<WorkSpaceUser> WorkSpaceUsers { get; set; }

        /// <summary>
        /// BoardUsers
        /// </summary>
        public virtual ICollection<BoardUser> BoardUsers { get; set; }

        /// <summary>
        /// WorkItems
        /// </summary>
        public virtual ICollection<WorkItem> WorkItems { get; set; }

        /// <summary>
        /// Comments
        /// </summary>
        public virtual ICollection<Comment> Comments { get; set; }

        /// <summary>
        /// Notifications
        /// </summary>
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
