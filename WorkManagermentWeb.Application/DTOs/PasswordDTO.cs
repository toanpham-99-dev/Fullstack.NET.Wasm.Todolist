using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using WorkManagermentWeb.Application.Attributes;
using WorkManagermentWeb.Domain.Resources;

namespace WorkManagermentWeb.Application.DTOs
{
    /// <summary>
    /// PasswordDTO
    /// </summary>
    public class PasswordDTO
    {
        /// <summary>
        /// UserId
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// CurrentPassword
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(Annotation))]
        [Display(Name = nameof(CurrentPassword), ResourceType = typeof(Annotation))]
        [RegularExpression(
            RegExConstants.PasswordRegEx,
            ErrorMessageResourceName = nameof(RegExConstants.PasswordErrorMessage),
            ErrorMessageResourceType = typeof(Annotation))]
        public string CurrentPassword { get; set; } = string.Empty;

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
        [Display(Name = nameof(ConfirmPassword), ResourceType = typeof(Annotation))]
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
    }
}
