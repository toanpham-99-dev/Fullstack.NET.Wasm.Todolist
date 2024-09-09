using System.Runtime.Serialization;

namespace WorkManagermentWeb.Domain.Enums
{
    /// <summary>
    /// WorkItemType
    /// </summary>
    public enum WorkItemType
    {
        /// <summary>
        /// Feature
        /// </summary>
        [EnumMember(Value = "Chức năng (Feature)")]
        Feature,

        /// <summary>
        /// Bug
        /// </summary>
        [EnumMember(Value = "Lỗi (Bug)")]
        Bug,

        /// <summary>
        /// Improve
        /// </summary>
        [EnumMember(Value = "Cải thiện (Improve)")]
        Improve,

        /// <summary>
        /// Refactor
        /// </summary>
        [EnumMember(Value = "Tái cấu trúc (Refactor)")]
        Refactor,

        /// <summary>
        /// Support
        /// </summary>
        [EnumMember(Value = "Hỗ trợ (Support)")]
        Support,

        /// <summary>
        /// Task
        /// </summary>
        [EnumMember(Value = "Nhiệm vụ (Task)")]
        Task,

        /// <summary>
        /// Release
        /// </summary>
        [EnumMember(Value = "Phát hành (Release)")]
        Release
    }
}
