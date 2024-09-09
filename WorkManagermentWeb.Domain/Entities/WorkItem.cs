using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkManagermentWeb.Domain.Entities.Base;
using WorkManagermentWeb.Domain.Enums;
#nullable disable

namespace WorkManagermentWeb.Domain.Entities
{
    /// <summary>
    /// WorkItem
    /// </summary>
    public class WorkItem : BaseModel
    {
        /// <summary>
        /// Code
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Code { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        [Required]
        [StringLength(250)]
        public string Title { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        [Required]
        public WorkItemType Type { get; set; }

        /// <summary>
        /// LastUpdaterId
        /// </summary>
        public string LastUpdaterId { get; set; }

        /// <summary>
        /// LastUpdaterName
        /// </summary>
        public string LastUpdaterName { get; set; }

        /// <summary>
        /// CreatorId
        /// </summary>
        public string CreatorId { get; set; } = string.Empty;

        /// <summary>
        /// CreatorName
        /// </summary>
        public string CreatorName { get; set; } = string.Empty;

        /// <summary>
        /// AssigneeId
        /// </summary>
        public string AssigneeId { get; set; }

        /// <summary>
        /// AssigneeName
        /// </summary>
        public string AssigneeName { get; set; }

        /// <summary>
        /// IsSyncToCalendar
        /// </summary>
        public bool IsSyncToCalendar { get; set; }

        /// <summary>
        /// BoardId
        /// </summary>
        public Guid BoardId { get; set; }

        /// <summary>
        /// Priority
        /// </summary>
        [Required]
        public WorkItemPriority Priority { get; set; }

        /// <summary>
        /// ParentWorkItem
        /// </summary>
        public Guid ParentWorkItemId { get; set; }

        /// <summary>
        /// StartDate
        /// </summary>
        public DateOnly? StartDate { get; set; }

        /// <summary>
        /// EndDate
        /// </summary>
        public DateOnly? EndDate { get; set; }

        /// <summary>
        /// WorkRemain
        /// </summary>
        public double WorkRemain { get; set; }

        /// <summary>
        /// User
        /// </summary>
        public virtual ApplicationUser User { get; set; }

        /// <summary>
        /// Board
        /// </summary>
        public virtual Board Board { get; set; }

        /// <summary>
        /// ParentWorkItem
        /// </summary>
        public virtual WorkItem ParentWorkItem { get; set; }

        /// <summary>
        /// Comments
        /// </summary>
        public virtual ICollection<Comment> Comments { get; set; }

        /// <summary>
        /// SubWorkItems
        /// </summary>
        public virtual ICollection<WorkItem> SubWorkItems { get; set; }

        /// <summary>
        /// CalendarEvent
        /// </summary>
        public virtual CalendarEvent CalendarEvent { get; set; }
    }
}
