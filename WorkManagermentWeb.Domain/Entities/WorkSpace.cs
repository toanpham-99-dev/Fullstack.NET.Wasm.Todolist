using System.ComponentModel.DataAnnotations;
using WorkManagermentWeb.Domain.Entities.Base;
#nullable disable

namespace WorkManagermentWeb.Domain.Entities
{
    /// <summary>
    /// WorkSpace
    /// </summary>
    public class WorkSpace : BaseModel
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Owner
        /// </summary>
        [Required]
        public string Owner { get; set; }

        /// <summary>
        /// WorkSpaceUsers
        /// </summary>
        public virtual ICollection<WorkSpaceUser> WorkSpaceUsers { get; set; }

        /// <summary>
        /// Boards
        /// </summary>
        public virtual ICollection<Board> Boards { get; set; }
    }
}
