#nullable disable

using System.ComponentModel.DataAnnotations;

namespace WorkManagermentWeb.Domain.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class CalendarEvent
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// WorkItemId
        /// </summary>
        public Guid WorkItemId { get; set; }

        /// <summary>
        /// IsSynced
        /// </summary>
        public bool IsSynced { get; set; }

        /// <summary>
        /// WorkItem
        /// </summary>
        public virtual WorkItem WorkItem { get; set; }
    }
}
