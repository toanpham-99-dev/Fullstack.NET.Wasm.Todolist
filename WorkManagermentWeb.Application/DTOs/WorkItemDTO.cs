using System.ComponentModel.DataAnnotations;
using WorkManagermentWeb.Domain.Enums;

namespace WorkManagermentWeb.Application.DTOs
{
    /// <summary>
    /// WorkItemDTO
    /// </summary>
    public class WorkItemDTO
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; } = Guid.Empty;

        /// <summary>
        /// Code
        /// </summary>
        public int? Code { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        [Required]
        [StringLength(250)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Type
        /// </summary>
        [Required]
        public WorkItemType Type { get; set; }

        /// <summary>
        /// LastUpdaterId
        /// </summary>
        public string? LastUpdaterId { get; set; }

        /// <summary>
        /// LastUpdaterName
        /// </summary>
        public string? LastUpdaterName { get; set; }

        /// <summary>
        /// CreatorId
        /// </summary>
        public string CreatorId { get; set; } = string.Empty;

        /// <summary>
        /// CreatorName
        /// </summary>
        public string CreatorName { get; set; } = string.Empty;

        /// <summary>
        /// Status
        /// </summary>
        public WorkItemStatus Status { get; set; } = WorkItemStatus.Todo;

        /// <summary>
        /// AssigneeId
        /// </summary>
        public string? AssigneeId { get; set; }

        /// <summary>
        /// AssigneeName
        /// </summary>
        public string? AssigneeName { get; set; }

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
        /// ParentWorkItemCode
        /// </summary>
        public int? ParentWorkItemCode { get; set; }

        /// <summary>
        /// StartDate
        /// </summary>
        public DateOnly? StartDate { get; set; }

        /// <summary>
        /// EndDate
        /// </summary>
        public DateOnly? EndDate { get; set; }

        /// <summary>
        /// CreatedDate
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// LastUpdatedDate
        /// </summary>
        public DateTime? LastUpdatedDate { get; set; }

        /// <summary>
        /// WorkRemain
        /// </summary>
        public double WorkRemain { get; set; }

        /// <summary>
        /// ProcessPercent
        /// </summary>
        public double ProcessPercent { get; set; }

        /// <summary>
        /// IsSyncToCalendar
        /// </summary>
        public bool IsSyncToCalendar { get; set; }

        /// <summary>
        /// SubWorkItems
        /// </summary>
        public List<SubWorkItem> SubWorkItems { get; set; } = new List<SubWorkItem>();

        /// <summary>
        /// Board
        /// </summary>
        public BoardInfo Board { get; set; } = new();
    }

    /// <summary>
    /// BoardInfo
    /// </summary>
    public class BoardInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }

    /// <summary>
    /// SubWorkItem
    /// </summary>
    public class SubWorkItem
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Code
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// StartDate
        /// </summary>
        public DateOnly? StartDate { get; set; }

        /// <summary>
        /// EndDate
        /// </summary>
        public DateOnly? EndDate { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public WorkItemStatus Status { get; set; } = WorkItemStatus.Todo;

        /// <summary>
        /// AssigneeId
        /// </summary>
        public string? AssigneeId { get; set; }

        /// <summary>
        /// AssigneeName
        /// </summary>
        public string? AssigneeName { get; set; }
    }
}
