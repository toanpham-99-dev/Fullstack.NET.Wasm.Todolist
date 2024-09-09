using System.ComponentModel.DataAnnotations;
using WorkManagermentWeb.Application.Attributes.Models;
using WorkManagermentWeb.Application.Utilities;
#nullable disable

namespace WorkManagermentWeb.Application.Attributes
{
    /// <summary>
    /// CustomPasswordAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = true)]
    public class CustomPasswordAttribute : ValidationAttribute
    {
        /// <summary>
        /// attributeRequireds
        /// </summary>
        private static readonly List<AttributeRequired> _attributeRequireds = new List<AttributeRequired>()
        {
            new AttributeRequired(RegExConstants.LowerCaseRegEx, RegExConstants.LowerCaseErrorMessage),
            new AttributeRequired(RegExConstants.UpperCaseRegEx, RegExConstants.UpperCaseErrorMessage),
            new AttributeRequired(RegExConstants.DigitRegEx, RegExConstants.DigitErrorMessage),
            new AttributeRequired(RegExConstants.SpecCharRegEx, RegExConstants.SpecCharErrorMessage),
            new AttributeRequired(RegExConstants.PasswordLengthRegEx, RegExConstants.PasswordLengthErrorMessage),
        };

        /// <summary>
        /// MultipleRegexAttribute
        /// </summary>
        /// <param name="pattern"></param>
        public CustomPasswordAttribute()
        {
        }

        /// <summary>
        /// IsValid
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            foreach (AttributeRequired _attribute in _attributeRequireds)
            {
                if (!value!.ToString()!.IsInputMatchRegex(_attribute.Condition))
                {
                    return new ValidationResult(_attribute.ErrorMessage);
                }
            }
            return ValidationResult.Success!;
        }
    }
}
