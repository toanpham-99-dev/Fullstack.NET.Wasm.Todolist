namespace WorkManagermentWeb.EventHandler.Events
{
    /// <summary>
    /// WorkItemUpdate
    /// </summary>
    public class WorkItemUpdate : IEvent
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
        /// UpdaterName
        /// </summary>
        public string UpdaterName { get; set; } = string.Empty;
    }
}
