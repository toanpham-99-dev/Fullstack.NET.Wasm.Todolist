#nullable disable

using System.ComponentModel.DataAnnotations;

namespace WorkManagermentWeb.Domain.Entities
{
    /// <summary>
    /// WorkSpaceUser
    /// </summary>
    public class WorkSpaceUser
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// WorkSpaceId
        /// </summary>
        [Required]
        public Guid WorkSpaceId { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        [Required]
        public string UserId { get; set; }

        /// <summary>
        /// WorkSpace
        /// </summary>
        public virtual WorkSpace WorkSpace { get; set; }

        /// <summary>
        /// User
        /// </summary>
        public virtual ApplicationUser User { get; set; }
    }
}
