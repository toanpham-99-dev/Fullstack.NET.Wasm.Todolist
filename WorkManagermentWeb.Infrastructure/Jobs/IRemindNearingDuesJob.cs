namespace WorkManagermentWeb.Infrastructure.Jobs
{
    /// <summary>
    /// IRemindNearingDuesJob
    /// </summary>
    public interface IRemindNearingDuesJob
    {
        /// <summary>
        /// RunAsync
        /// </summary>
        /// <returns></returns>
        Task RunAsync();
    }
}
