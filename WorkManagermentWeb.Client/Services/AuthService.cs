using Blazored.LocalStorage;
using System.ComponentModel;
using System.Security.Claims;
using WorkManagermentWeb.Application.Constants;
using WorkManagermentWeb.Application.DTOs;
using WorkManagermentWeb.Application.DTOs.Responses;
using WorkManagermentWeb.Application.GenericModels;
using WorkManagermentWeb.Application.Utilities;
using WorkManagermentWeb.Client.Interfaces;

namespace WorkManagermentWeb.Client.Services
{
    /// <summary>
    /// AuthService
    /// </summary>
    public class AuthService : IAuthService
    {
        /// <summary>
        /// _isAuthenticated
        /// </summary>
        private bool _isAuthenticated;

        /// <summary>
        /// OnAuthStateChanged
        /// </summary>
        public event Action OnAuthStateChanged;

        /// <summary>
        /// _type
        /// </summary>
        private string _type;

        /// <summary>
        /// Type
        /// </summary>
        public string Type
        {
            get => _type;
            set
            {
                if (_type != value)
                {
                    _type = value;
                    NotifyAuthStateChanged();
                }
            }
        }

        /// <summary>
        /// _user
        /// </summary>
        private UserDTO _user;

        /// <summary>
        /// User
        /// </summary>
        public UserDTO User
        {
            get => _user;
            set
            {
                if (_user != value)
                {
                    _user = value;
                    NotifyAuthStateChanged();
                }
            }
        }

        /// <summary>
        /// ILocalStorageService
        /// </summary>
        private readonly ILocalStorageService _localStorageService;

        /// <summary>
        /// ISyncLocalStorageService
        /// </summary>
        private readonly ISyncLocalStorageService _syncLocalStorageService;

        /// <summary>
        /// AuthService
        /// </summary>
        /// <param name="localStorageService"></param>
        /// <param name="syncLocalStorageService"></param>
        /// <param name="httpClient"></param>
        /// <param name="configuration"></param>
        public AuthService(
            ILocalStorageService localStorageService,
            ISyncLocalStorageService syncLocalStorageService)
        {
            _localStorageService = localStorageService;
            _syncLocalStorageService = syncLocalStorageService;
            InitAuthState();
        }

        /// <summary>
        /// InitAuthState
        /// </summary>
        public void InitAuthState()
        {
            try
            {
                string? stringToken = _syncLocalStorageService.GetItemAsString(AuthorizationConstants.Token);
                string? stringGraphToken = _syncLocalStorageService.GetItemAsString(AuthorizationConstants.GraphToken);

                if (string.IsNullOrWhiteSpace(stringToken))
                {
                    User = new UserDTO();
                    IsAuthenticated = false;
                    return;
                }

                var userSession = Generics.GetClaimsFromToken(stringToken!);

                var claimsPrincipal = Generics.SetClaimPrincipal(userSession);
                User = GetUser(claimsPrincipal);
                IsAuthenticated = true;
                Type = string.IsNullOrEmpty(stringGraphToken) ? AuthenticationTypes.Password : AuthenticationTypes.Negotiate;
            }
            catch
            {
                User = new UserDTO();
                IsAuthenticated = false;
            }
        }

        /// <summary>
        /// UpdateAuthenticationState
        /// </summary>
        /// <param name="token"></param>
        /// <param name="graphToken"></param>
        /// <returns></returns>
        public async Task UpdateAuthenticationState(string? token, string? graphToken = null)
        {
            ClaimsPrincipal claimsPrincipal = new();
            if (string.IsNullOrWhiteSpace(token))
            {
                User = new UserDTO();
                IsAuthenticated = false;
                Type = string.Empty;
                await _localStorageService.RemoveItemAsync(AuthorizationConstants.Token);
                await _localStorageService.RemoveItemAsync(AuthorizationConstants.GraphToken);
                return;
            }
            var userSession = Generics.GetClaimsFromToken(token!);
            claimsPrincipal = Generics.SetClaimPrincipal(userSession);
            User = GetUser(claimsPrincipal);
            IsAuthenticated = true;
            Type = AuthenticationTypes.Password;
            await _localStorageService.SetItemAsStringAsync(AuthorizationConstants.Token, token!);

            if (!string.IsNullOrWhiteSpace(graphToken))
            {
                Type = AuthenticationTypes.Negotiate;
                await _localStorageService.SetItemAsStringAsync(AuthorizationConstants.GraphToken, graphToken);
            }
        }

        /// <summary>
        /// IsAuthenticated
        /// </summary>
        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            set
            {
                if (_isAuthenticated != value)
                {
                    _isAuthenticated = value;
                    NotifyAuthStateChanged();
                }
            }
        }

        /// <summary>
        /// NotifyAuthStateChanged
        /// </summary>
        private void NotifyAuthStateChanged() => OnAuthStateChanged?.Invoke();

        /// <summary>
        /// GetUser
        /// </summary>
        private UserDTO GetUser(ClaimsPrincipal claimsPrincipal)
        {
            string roleName = claimsPrincipal!.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Role)!.Value.ToString();
            return new()
            {
                Id = Helper.GetCurrentUserId(claimsPrincipal!)!,
                UserName = claimsPrincipal!.Identity!.Name!,
                Role = new RoleDTO
                {
                    Value = roleName,
                    DisplayName = Helper.GetConstFieldAttributeValue<UserRoleConstants, string, DescriptionAttribute>(roleName, y => y.Description)
                }
            };
        }

        /// <summary>
        /// GetGraphToken
        /// </summary>
        /// <returns></returns>
        public async Task<string?> GetGraphToken()
        {
            string? stringGraphToken = await _localStorageService.GetItemAsStringAsync(AuthorizationConstants.GraphToken);
            return stringGraphToken;
        }
    }
}
