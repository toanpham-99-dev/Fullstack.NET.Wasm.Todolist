using System.ComponentModel.DataAnnotations;

namespace WorkManagermentWeb.Application.DTOs
{
    /// <summary>
    /// RequestResetPasswordTokenDTO
    /// </summary>
    public class RequestResetPasswordTokenDTO
    {
        /// <summary>
        /// Email
        /// </summary>
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

    }
}
