using System.ComponentModel.DataAnnotations;
#nullable disable

namespace WorkManagermentWeb.Application.Attributes
{
    /// <summary>
    /// GreaterThanOrEqualAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class CustomGreaterThanOrEqualAttribute : ValidationAttribute
    {
        /// <summary>
        /// _comparisonProperty
        /// </summary>
        private readonly string _comparisonProperty;

        /// <summary>
        /// GreaterThanOrEqualAttribute
        /// </summary>
        /// <param name="comparisonProperty"></param>
        public CustomGreaterThanOrEqualAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        /// <summary>
        /// IsValid
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return new ValidationResult("Invalid entry");
            ErrorMessage = ErrorMessageString;

            if (value.GetType() == typeof(IComparable)) throw new ArgumentException("value has not implemented IComparable interface");
            var currentValue = (IComparable)value;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null) throw new ArgumentException("Comparison property with this name not found");

            var comparisonValue = property.GetValue(validationContext.ObjectInstance);
            if (!ReferenceEquals(value.GetType(), comparisonValue.GetType()))
                throw new ArgumentException("The types of the fields to compare are not the same.");

            return currentValue.CompareTo((IComparable)comparisonValue) >= 0 ? ValidationResult.Success : new ValidationResult(ErrorMessage);
        }
    }
}
