using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using WorkManagermentWeb.Application.Attributes;
using WorkManagermentWeb.Domain.Resources;
using WorkManagermentWeb.Domain.Enums;

namespace WorkManagermentWeb.Application.DTOs
{
    /// <summary>
    /// PostPutBoardDTO
    /// </summary>
    public class PostPutBoardDTO
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

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
        /// LastUpdateBy
        /// </summary>
        public string LastUpdateBy { get; set; } = string.Empty;

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
        /// AssigneeId
        /// </summary>
        public string AssigneeId { get; set; } = string.Empty;

        /// <summary>
        /// AssigneeName
        /// </summary>
        [Display(Name = nameof(AssigneeName), ResourceType = typeof(Annotation))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(Annotation))]
        public string AssigneeName { get; set; } = string.Empty;

        /// <summary>
        /// Status
        /// </summary>
        public BoardStatus Status { get; set; } = 0;

        /// <summary>
        /// WorkSpaceId
        /// </summary>
        public Guid WorkSpaceId { get; set; }
    }
}
