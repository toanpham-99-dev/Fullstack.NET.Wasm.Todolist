namespace WorkManagermentWeb.Infrastructure.Services
{
    /// <summary>
    /// ISMSSender
    /// </summary>
    public interface ISmsSender
    {
        /// <summary>
        /// SendAsync
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendAsync(string phone, string message);
    }
}
