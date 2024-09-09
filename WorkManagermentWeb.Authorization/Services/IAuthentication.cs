using WorkManagermentWeb.Application.DTOs;

namespace WorkManagermentWeb.Authorization.Services
{
    /// <summary>
    /// IAuthentication
    /// </summary>
    public interface IAuthentication
    {
        /// <summary>
        /// GenerateJwtToken
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        string GenerateJwtToken(UserSession user);
    }
}
