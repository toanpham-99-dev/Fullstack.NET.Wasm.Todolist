using System.ComponentModel;

namespace WorkManagermentWeb.Application.Constants
{
    /// <summary>
    /// UserRoleConstants
    /// </summary>
    public class UserRoleConstants
    {
        /// <summary>
        /// CEO
        /// </summary>
        [Description(nameof(CEO))]
        //[Description("Chủ tịch (CEO)")]
        public const string CEO = nameof(CEO);

        /// <summary>
        /// ProjectManager
        /// </summary>
        [Description(nameof(ProjectManager))]
        //[Description("Quản lý (Project Manager)")]
        public const string ProjectManager = nameof(ProjectManager);

        /// <summary>
        /// Member
        /// </summary>
        [Description(nameof(Member))]
        //[Description("Nhân viên (Member)")]
        public const string Member = nameof(Member);

        /// <summary>
        /// UserRoles
        /// </summary>
        public static List<string> UserRoles { get; set; } = new List<string>() { CEO, ProjectManager, Member };
    }
}
