using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using WorkManagermentWeb.Application.Attributes;
using WorkManagermentWeb.Application.DTOs.Responses;
using WorkManagermentWeb.Domain.Resources;

namespace WorkManagermentWeb.Application.DTOs
{
    /// <summary>
    /// UpdateUserDTO
    /// </summary>
    public class UpdateUserDTO
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; } = string.Empty;

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
        /// Phone
        /// </summary>
        [RegularExpression(
            RegExConstants.PhoneRegEx,
            ErrorMessageResourceName = nameof(RegExConstants.PhoneErrorMessage),
            ErrorMessageResourceType = typeof(Annotation))]
        [Display(Name = nameof(Phone), ResourceType = typeof(Annotation))]
        public string? Phone { get; set; } = string.Empty;

        /// <summary>
        /// IsPhoneConfirmed
        /// </summary>
        public bool IsPhoneConfirmed { get; set; }

        /// <summary>
        /// Role
        /// </summary>
        [Display(Name = nameof(Role), ResourceType = typeof(Annotation))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(Annotation))]
        public RoleDTO Role { get; set; } = new();
    }
}
