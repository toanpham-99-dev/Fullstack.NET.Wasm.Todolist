namespace WorkManagermentWeb.Pusher.Options
{
    /// <summary>
    /// PusherOptions
    /// </summary>
    public class PusherOptions
    {
        /// <summary>
        ///     AppId
        /// </summary>
        public string AppId { get; set; } = string.Empty;     

        /// <summary>
        ///     AppKey
        /// </summary>
        public string AppKey { get; set; } = string.Empty;

        /// <summary>
        ///     AppSecret
        /// </summary>
        public string AppSecret { get; set; } = string.Empty;

        /// <summary>
        ///     AppCluster
        /// </summary>
        public string AppCluster { get; set; } = string.Empty;

        /// <summary>
        ///     EventName
        /// </summary>
        public string EventName { get; set; } = string.Empty; 
    }
}
