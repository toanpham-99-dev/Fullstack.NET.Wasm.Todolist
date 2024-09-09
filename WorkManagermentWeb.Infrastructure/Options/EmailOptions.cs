namespace WorkManagermentWeb.Infrastructure.Options
{
    /// <summary>
    /// EmailOptions
    /// </summary>
    public class EmailOptions
    {
        /// <summary>
        /// Mail
        /// </summary>
        public string Mail { get; set; } = string.Empty;

        /// <summary>
        /// DisplayName
        /// </summary>
        public string DisplayName { get; set; } = string.Empty; 

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Host
        /// </summary>
        public string Host { get; set; } = string.Empty;

        /// <summary>
        /// Port
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// ApiUrl
        /// </summary>
        public string ApiUrl { get; set; } = string.Empty;

        /// <summary>
        /// ApiToken
        /// </summary>
        public string ApiToken { get; set; } = string.Empty;
    }
}
