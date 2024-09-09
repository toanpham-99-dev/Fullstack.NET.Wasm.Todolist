namespace WorkManagermentWeb.Infrastructure.Options
{
    /// <summary>
    /// RemindNearingDuesOptions
    /// </summary>
    public class RemindNearingDuesOptions
    {
        /// <summary>
        /// JobId
        /// </summary>
        public string JobId { get; set; } = string.Empty;

        /// <summary>
        /// CronExpression
        /// </summary>
        public string CronExpression { get; set; } = string.Empty;
    }
}
