namespace WorkManagermentWeb.EventHandler.Events
{
    /// <summary>
    /// WorkSpaceMemberRemove
    /// </summary>
    public class WorkSpaceMemberRemove : IEvent
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
        /// WorkSpaceName
        /// </summary>
        public string WorkSpaceName { get; set; } = string.Empty;

        /// <summary>
        /// UpdaterName
        /// </summary>
        public string UpdaterName { get; set; } = string.Empty;
    }
}
