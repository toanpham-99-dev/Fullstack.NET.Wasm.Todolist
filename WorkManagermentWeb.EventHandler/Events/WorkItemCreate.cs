namespace WorkManagermentWeb.EventHandler.Events
{
    /// <summary>
    /// WorkItemCreate
    /// </summary>
    public class WorkItemCreate : IEvent
    {
        /// <summary>
        /// ObjectId
        /// </summary>
        public string ObjectId { get; set; } = string.Empty;

        /// <summary>
        /// RecieverId
        /// </summary>
        public string RecieverId { get; set; } = string.Empty;

        /// <summary>
        /// WorkItemCode
        /// </summary>
        public string WorkItemCode { get; set; } = string.Empty;

        /// <summary>
        /// WorkItemTitle
        /// </summary>
        public string WorkItemTitle { get; set; } = string.Empty;

        /// <summary>
        /// CreatorName
        /// </summary>
        public string CreatorName { get; set; } = string.Empty;
    }
}
