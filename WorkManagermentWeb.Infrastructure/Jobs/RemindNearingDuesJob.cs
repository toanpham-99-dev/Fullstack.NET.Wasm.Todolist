using WorkManagermentWeb.Application.Contracts;

namespace WorkManagermentWeb.Infrastructure.Jobs
{
    /// <summary>
    /// RemindNearingDuesJob
    /// </summary>
    public class RemindNearingDuesJob : IRemindNearingDuesJob
    {
        /// <summary>
        /// IUser
        /// </summary>
        private readonly IUser _userRepository;

        /// <summary>
        /// RemindNearingDuesJob
        /// </summary>
        /// <param name="userRepository"></param>
        public RemindNearingDuesJob(IUser userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// RunAsync
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task RunAsync()
        {
            await _userRepository.RemindNearingDuesAsync();
        }
    }
}
