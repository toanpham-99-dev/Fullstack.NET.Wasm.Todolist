using System.Runtime.Serialization;

namespace WorkManagermentWeb.Domain.Enums
{
    /// <summary>
    /// WorkItemStatus
    /// </summary>
    public enum WorkItemStatus
    {
        /// <summary>
        /// Todo
        /// </summary>
        [EnumMember(Value = "Chưa xử lý (To do)")]
        Todo,

        /// <summary>
        /// Processing
        /// </summary>
        [EnumMember(Value = "Đang xử lý (Processing)")]
        Processing,

        /// <summary>
        /// Resolved
        /// </summary>
        [EnumMember(Value = "Đã xử lý (Resolved)")]
        Resolved,

        /// <summary>
        /// Done
        /// </summary>
        [EnumMember(Value = "Đã hoàn thành (Done)")]
        Done
    }
}
