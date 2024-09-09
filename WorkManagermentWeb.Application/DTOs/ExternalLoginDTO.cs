using WorkManagermentWeb.Application.Constants;

namespace WorkManagermentWeb.Application.DTOs
{
    /// <summary>
    /// ExternalLoginDTO
    /// </summary>
    public class ExternalLoginDTO
    {
        /// <summary>
        /// FullName
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// RoleName
        /// </summary>
        public string RoleName { get; set; } = UserRoleConstants.Member;

        /// <summary>
        /// GraphToken
        /// </summary>
        public string GraphToken { get; set; } = string.Empty;
    }
}
