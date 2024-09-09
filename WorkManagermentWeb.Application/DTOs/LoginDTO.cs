using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using WorkManagermentWeb.Application.Attributes;
using WorkManagermentWeb.Domain.Resources;

namespace WorkManagermentWeb.Application.DTOs
{
    /// <summary>
    /// LoginDTO
    /// </summary>
    public class LoginDTO
    {
        /// <summary>
        /// Email
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(Annotation))]
        [EmailAddress(
            ErrorMessageResourceName = "EmailErrorMessage",
            ErrorMessageResourceType = typeof(Annotation))]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Password
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(Annotation))]
        [Display(Name = nameof(Password), ResourceType = typeof(Annotation))]
        [RegularExpression(
            RegExConstants.PasswordRegEx,
            ErrorMessageResourceName = nameof(RegExConstants.PasswordErrorMessage),
            ErrorMessageResourceType = typeof(Annotation))]
        public string Password { get; set; } = string.Empty;
    }
}
