using System.ComponentModel.DataAnnotations;
using WorkManagermentWeb.Domain.Entities.Base;
#nullable disable

namespace WorkManagermentWeb.Domain.Entities
{
    /// <summary>
    /// Board
    /// </summary>
    public class Board : BaseModel
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// StartDate
        /// </summary>
        public DateOnly StartDate { get; set; }

        /// <summary>
        /// EndDate
        /// </summary>
        public DateOnly EndDate { get; set; }

        /// <summary>
        /// AssigneeId
        /// </summary>
        public string AssigneeId { get; set; }

        /// <summary>
        /// AssigneeName
        /// </summary>
        public string AssigneeName { get; set; }

        /// <summary>
        /// LastUpdateBy
        /// </summary>
        [Required]
        public string LastUpdateBy { get; set; }

        /// <summary>
        /// WorkSpaceId
        /// </summary>
        public Guid WorkSpaceId { get; set; }

        /// <summary>
        /// BoardUsers
        /// </summary>
        public virtual ICollection<BoardUser> BoardUsers { get; set; }

        /// <summary>
        /// WorkItems
        /// </summary>
        public virtual ICollection<WorkItem> WorkItems { get; set; }

        /// <summary>
        /// WorkSpace
        /// </summary>
        public virtual WorkSpace WorkSpace { get; set; }
    }
}
