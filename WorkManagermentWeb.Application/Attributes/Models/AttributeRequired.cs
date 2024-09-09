namespace WorkManagermentWeb.Application.Attributes.Models
{
    /// <summary>
    /// AttributeRequired
    /// </summary>
    public class AttributeRequired
    {
        /// <summary>
        /// Condition
        /// </summary>
        public string Condition { get; }

        /// <summary>
        /// ErrorMessage
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// AttributeRequired
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="errorMessage"></param>
        public AttributeRequired(string condition, string errorMessage)
        {
            Condition = condition;
            ErrorMessage = errorMessage;
        }
    }
}
