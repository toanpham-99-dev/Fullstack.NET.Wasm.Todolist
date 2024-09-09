using System.Runtime.Serialization;

namespace WorkManagermentWeb.Domain.Enums
{
    /// <summary>
    /// BoardStatus
    /// </summary>
    public enum BoardStatus
    {
        /// <summary>
        /// NotStarted
        /// </summary>
        [EnumMember(Value = "Chưa bắt đầu (NotStarted)")]
        NotStarted,

        /// <summary>
        /// Running
        /// </summary>
        [EnumMember(Value = "Đang chạy (Inprogress)")]
        Inprogress,

        /// <summary>
        /// Stopped
        /// </summary>
        [EnumMember(Value = "Đã hoàn thành (Completed)")]
        Completed
    }
}
