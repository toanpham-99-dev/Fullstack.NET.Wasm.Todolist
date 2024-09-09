namespace WorkManagermentWeb.Application.DTOs.Requests
{
    /// <summary>
    /// ApiEmailRequest
    /// </summary>
    public class ApiEmailRequest
    {
        /// <summary>
        /// Subject
        /// </summary>
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// Html
        /// </summary>
        public string Html { get; set; } = string.Empty;

        /// <summary>
        /// From
        /// </summary>
        public From From { get; set; } = new From();

        /// <summary>
        /// To
        /// </summary>
        public List<From> To { get; set; } = new List<From>();
    }

    /// <summary>
    /// From
    /// </summary>
    public class From
    {
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Name
        /// </summary>
        public string Name {  get; set; } = string.Empty;
    }
}
