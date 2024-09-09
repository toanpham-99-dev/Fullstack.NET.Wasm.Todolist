namespace WorkManagermentWeb.Application.DTOs
{
    /// <summary>
    /// UserNearingDue
    /// </summary>
    public class UserNearingDue
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// FullName
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Email
        /// </summary>
        public string Email {  get; set; } = string.Empty;

        /// <summary>
        /// WorkItems
        /// </summary>
        public List<SubWorkItem> WorkItems { get; set; } = new List<SubWorkItem>();
    }
}
