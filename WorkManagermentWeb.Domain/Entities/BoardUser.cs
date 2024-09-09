#nullable disable

using System.ComponentModel.DataAnnotations;

namespace WorkManagermentWeb.Domain.Entities
{
    /// <summary>
    /// BoardUser
    /// </summary>
    public class BoardUser
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// BoardId
        /// </summary>
        [Required]
        public Guid BoardId { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        [Required]
        public string UserId { get; set; }

        /// <summary>
        /// Board
        /// </summary>
        public virtual Board Board { get; set; }

        /// <summary>
        /// User
        /// </summary>
        public virtual ApplicationUser User { get; set; }
    }
}
