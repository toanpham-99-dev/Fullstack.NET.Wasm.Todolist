using WorkManagermentWeb.Application.DTOs.Responses;
using WorkManagermentWeb.Domain.Enums;

namespace WorkManagermentWeb.Client.Models
{
    /// <summary>
    /// WorkItemSelectedFilter
    /// </summary>
    public class WorkItemSelectedFilter
    {
        /// <summary>
        /// SeletedCreatorName
        /// </summary>
        public string? SeletedCreatorName { get; set; }

        /// <summary>
        /// SelectedAssigneeName
        /// </summary>
        public string? SelectedAssigneeName { get; set; }

        /// <summary>
        /// SelectedPriority
        /// </summary>
        public PropertyInfo<WorkItemPriority>? SelectedPriority { get; set; }

        /// <summary>
        /// SelectedCategory
        /// </summary>
        public PropertyInfo<WorkItemType>? SelectedCategory { get; set; }

        /// <summary>
        /// SelectedStatus
        /// </summary>
        public PropertyInfo<WorkItemStatus>? SelectedStatus { get; set; }

    }
}
