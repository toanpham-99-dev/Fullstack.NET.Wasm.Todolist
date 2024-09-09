namespace WorkManagermentWeb.EventHandler.Events
{
    /// <summary>
    /// WorkSpaceMemberAdd
    /// </summary>
    public class WorkSpaceMemberAdd : IEvent
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
