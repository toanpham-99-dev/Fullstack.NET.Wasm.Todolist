using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using WorkManagermentWeb.Application.Attributes;
using WorkManagermentWeb.Application.Constants;
using WorkManagermentWeb.Domain.Resources;

namespace WorkManagermentWeb.Application.DTOs
{
    /// <summary>
    /// RegisterUserDTO
    /// </summary>
    public class RegisterUserDTO
    {
        /// <summary>
        /// FullName
        /// </summary>
        [RegularExpression(
            RegExConstants.FullNameRegEx,
            ErrorMessageResourceName = nameof(RegExConstants.FullNameErrorMessage),
            ErrorMessageResourceType = typeof(Annotation))]
        [Display(Name = nameof(FullName), ResourceType = typeof(Annotation))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(Annotation))]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// UserName
        /// </summary>
        [RegularExpression(
            RegExConstants.UsernameRegEx,
            ErrorMessageResourceName = nameof(RegExConstants.UsernameErrorMessage),
            ErrorMessageResourceType = typeof(Annotation))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(Annotation))]
        [Display(Name = nameof(UserName), ResourceType = typeof(Annotation))]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Email
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(Annotation))]
        [EmailAddress(
            ErrorMessageResourceName = "EmailErrorMessage",
            ErrorMessageResourceType = typeof(Annotation))]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// IsEmailComfirmed
        /// </summary>
        public bool IsEmailComfirmed { get; set; }

        /// <summary>
        /// Phone
        /// </summary>
        [RegularExpression(
            RegExConstants.PhoneRegEx,
            ErrorMessageResourceName = nameof(RegExConstants.PhoneErrorMessage),
            ErrorMessageResourceType = typeof(Annotation))]
        [Display(Name = nameof(Phone), ResourceType = typeof(Annotation))]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// IsPhoneComfirmed
        /// </summary>
        public bool IsPhoneComfirmed { get; set; }

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
            nameof(Password),
            ErrorMessageResourceName = "PaswordCompare",
            ErrorMessageResourceType = typeof(Annotation))]
        public string ConfirmPassword { get; set; } = string.Empty;

        /// <summary>
        /// ActiveStatus
        /// </summary>
        [Required]
        public bool ActiveStatus { get; set; }

        /// <summary>
        /// RoleName
        /// </summary>
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(Annotation))]
        [Display(Name = nameof(RoleName), ResourceType = typeof(Annotation))]
        public string RoleName { get; set; } = UserRoleConstants.Member;
    }
}
