namespace WorkManagermentWeb.Background.Options
{
    /// <summary>
    /// BackgroundJobOptions
    /// </summary>
    public class BackgroundJobOptions
    {
        /// <summary>
        /// ConnectionString
        /// </summary>
        public string ConnectionString { get; set; } = string.Empty;

        /// <summary>
        /// HangfireRoute
        /// </summary>
        public string HangfireRoute { get; set; } = string.Empty;

        /// <summary>
        /// HangfireTitle
        /// </summary>
        public string HangfireTitle { get; set; } = string.Empty;

        /// <summary>
        /// Account
        /// </summary>
        public HangfireAuth Account { get; set; } = new HangfireAuth();
    }

    /// <summary>
    /// HangfireAuth
    /// </summary>
    public class HangfireAuth
    {
        /// <summary>
        /// UserName
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
