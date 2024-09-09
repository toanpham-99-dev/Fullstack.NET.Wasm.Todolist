namespace WorkManagermentWeb.EventHandler.Events
{
    /// <summary>
    /// IEvent
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        /// ObjectId
        /// </summary>
        public string ObjectId { get; set; }

        /// <summary>
        /// RecieverId
        /// </summary>
        public string RecieverId { get; set; }
    }
}
