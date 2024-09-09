using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using WorkManagermentWeb.Application.Attributes;
using WorkManagermentWeb.Domain.Resources;
using WorkManagermentWeb.Domain.Enums;

namespace WorkManagermentWeb.Application.DTOs
{
    /// <summary>
    /// PostPutWorkItemDTO
    /// </summary>
    public class PostPutWorkItemDTO
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
        [StringLength(250,
            ErrorMessageResourceName = "TitleErrorMessage",
            ErrorMessageResourceType = typeof(Annotation))]
        [Display(Name = nameof(Title), ResourceType = typeof(Annotation))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(Annotation))]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Type
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(Annotation))]
        public WorkItemType Type { get; set; }

        /// <summary>
        /// LastUpdaterId
        /// </summary>
        public string? LastUpdaterId { get; set; }

        /// <summary>
        /// CreatorId
        /// </summary>
        public string CreatorId { get; set; } = string.Empty;

        /// <summary>
        /// Status
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(Annotation))]
        public WorkItemStatus Status { get; set; } = WorkItemStatus.Todo;

        /// <summary>
        /// AssigneeId
        /// </summary>
        public string? AssigneeId { get; set; }

        /// <summary>
        /// AssigneeName
        /// </summary>
        [Display(Name = nameof(AssigneeName), ResourceType = typeof(Annotation))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(Annotation))]
        public string? AssigneeName { get; set; }

        /// <summary>
        /// BoardId
        /// </summary>
        public Guid BoardId { get; set; }

        /// <summary>
        /// Priority
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(Annotation))]
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
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(Annotation))]
        [Display(Name = nameof(StartDate), ResourceType = typeof(Annotation))]
        public DateTimeOffset? StartDate { get; set; } = new DateTimeOffset(DateTime.Now);

        /// <summary>
        /// EndDate
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(Annotation))]
        [Display(Name = nameof(EndDate), ResourceType = typeof(Annotation))]
        public DateTimeOffset? EndDate { get; set; } = new DateTimeOffset(DateTime.Now.AddDays(1));

        /// <summary>
        /// WorkRemain
        /// </summary>
        [RegularExpression(RegExConstants.NumberRegEx,
            ErrorMessageResourceName = nameof(RegExConstants.NumberErrorMessage),
            ErrorMessageResourceType = typeof(Annotation))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(Annotation))]
        [Display(Name = nameof(WorkRemain), ResourceType = typeof(Annotation))]
        public string WorkRemain { get; set; } = string.Empty;
    }
}
