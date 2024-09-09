using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using WorkManagermentWeb.Application.Attributes;
using WorkManagermentWeb.Domain.Resources;

namespace WorkManagermentWeb.Application.DTOs
{
    /// <summary>
    /// ResetPasswordDTO
    /// </summary>
    public class ResetPasswordDTO
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
        /// NewPassword
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(Annotation))]
        [Display(Name = nameof(NewPassword), ResourceType = typeof(Annotation))]
        [RegularExpression(
            RegExConstants.PasswordRegEx,
            ErrorMessageResourceName = nameof(RegExConstants.PasswordErrorMessage),
            ErrorMessageResourceType = typeof(Annotation))]
        public string NewPassword { get; set; } = string.Empty;

        /// <summary>
        /// ConfirmPassword
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(Annotation))]
        [RegularExpression(
            RegExConstants.PasswordRegEx,
            ErrorMessageResourceName = nameof(RegExConstants.PasswordErrorMessage),
            ErrorMessageResourceType = typeof(Annotation))]
        [Compare(
            nameof(NewPassword),
            ErrorMessageResourceName = "NewPaswordCompare",
            ErrorMessageResourceType = typeof(Annotation))]
        public string ConfirmPassword { get; set; } = string.Empty;

        /// <summary>
        /// Token
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(Annotation))]
        public string Token { get; set; } = string.Empty;
    }
}
