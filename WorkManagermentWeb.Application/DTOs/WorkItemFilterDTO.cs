using WorkManagermentWeb.Domain.Enums;

namespace WorkManagermentWeb.Application.DTOs
{
    /// <summary>
    /// WorkItemFilterDTO
    /// </summary>
    public class WorkItemFilterDTO
    {
        /// <summary>
        /// StartDate
        /// </summary>
        public DateOnly? StartDate { get; set; }// = DateOnly.FromDateTime(DateTime.Now.AddDays(1));

        /// <summary>
        /// EndDate
        /// </summary>
        public DateOnly? EndDate { get; set; }// = DateOnly.FromDateTime(DateTime.Now.AddDays(1));

        /// <summary>
        /// Status
        /// </summary>
        public WorkItemStatus? Status { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public WorkItemType? Type { get; set; }

        /// <summary>
        /// Priority
        /// </summary>
        public WorkItemPriority? Priority { get; set; }

        /// <summary>
        /// CreatedBy
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// TitleOrDescription
        /// </summary>
        public string? TitleOrDescription { get; set; }

        /// <summary>
        /// OwnerId
        /// </summary>
        public string? OwnerId { get; set; }

        /// <summary>
        /// BoardId
        /// </summary>
        public Guid BoardId { get; set; }

        /// <summary>
        /// Code
        /// </summary>
        public int? Code { get; set; }

        /// <summary>
        /// IsSyncToCalendar
        /// </summary>
        public bool? IsSyncToCalendar { get; set; }

        /// <summary>
        /// Paging
        /// </summary>
        public PagingDTO Paging { get; set; } = new PagingDTO();

        /// <summary>
        /// SortBy
        /// </summary>
        public SortingDTO? Sorting { get; set; } = new SortingDTO();
    }
}
