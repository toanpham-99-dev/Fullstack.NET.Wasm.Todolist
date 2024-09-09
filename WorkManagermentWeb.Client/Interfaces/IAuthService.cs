using Microsoft.AspNetCore.Components.Authorization;
using WorkManagermentWeb.Application.DTOs;

namespace WorkManagermentWeb.Client.Interfaces
{
    /// <summary>
    /// IAuthService
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// User
        /// </summary>
        UserDTO User { get; set; }

        /// <summary>
        /// IsAuthenticated
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// Type
        /// </summary>
        string Type { get; set; }

        /// <summary>
        /// OnAuthStateChanged
        /// </summary>
        event Action OnAuthStateChanged;

        /// <summary>
        /// UpdateAuthenticationState
        /// </summary>
        /// <param name="token"></param>
        /// <param name="graphToken"></param>
        /// <returns></returns>
        Task UpdateAuthenticationState(string? token, string? graphToken = null);

        /// <summary>
        /// GetGraphToken
        /// </summary>
        /// <returns></returns>
        Task<string?> GetGraphToken();
    }
}
