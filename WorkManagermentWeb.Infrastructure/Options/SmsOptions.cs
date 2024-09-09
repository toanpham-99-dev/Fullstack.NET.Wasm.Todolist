namespace WorkManagermentWeb.Infrastructure.Options
{
    /// <summary>
    /// SmsOptions
    /// </summary>
    public class SmsOptions
    {
        /// <summary>
        /// SMSAccountIdentification
        /// </summary>
        public string SMSAccountIdentification { get; set; } = string.Empty;

        /// <summary>
        /// SMSAccountPassword
        /// </summary>
        public string SMSAccountPassword { get; set; } = string.Empty;

        /// <summary>
        /// SMSAccountFrom
        /// </summary>
        public string SMSAccountFrom { get; set; } = string.Empty;
    }
}
