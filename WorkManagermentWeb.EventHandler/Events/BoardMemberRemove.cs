﻿namespace WorkManagermentWeb.EventHandler.Events
{
    /// <summary>
    /// BoardMemberRemove
    /// </summary>
    public class BoardMemberRemove : IEvent
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
        /// BoardName
        /// </summary>
        public string BoardName { get; set; } = string.Empty;

        /// <summary>
        /// UpdaterName
        /// </summary>
        public string UpdaterName { get; set; } = string.Empty;
    }
}
