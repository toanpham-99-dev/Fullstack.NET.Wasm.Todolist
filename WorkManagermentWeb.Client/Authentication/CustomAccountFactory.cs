using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Security.Claims;
using WorkManagermentWeb.Client.Models;
using WorkManagermentWeb.Client.Interfaces;
using WorkManagermentWeb.Application.Contracts;
using WorkManagermentWeb.Application.DTOs.Responses;
using WorkManagermentWeb.Application.DTOs;

namespace WorkManagermentWeb.Client.Authentication
{
    /// <summary>
    /// CustomAccountFactory
    /// </summary>
    /// <param name="accessor"></param>
    /// <param name="serviceProvider"></param>
    /// <param name="authService"></param>
    /// <param name="userService"></param>
    /// <param name="microsoftCalendarService"></param>
    public class CustomAccountFactory(
        IAccessTokenProviderAccessor accessor,
        IServiceProvider serviceProvider,
        IAuthService authService,
        IUser userService,
        IMicrosoftCalendarService microsoftCalendarService)
        : AccountClaimsPrincipalFactory<CustomUserAccount>(accessor)
    {
        /// <summary>
        /// IServiceProvider
        /// </summary>
        private readonly IServiceProvider serviceProvider = serviceProvider;

        /// <summary>
        /// AuthService
        /// </summary>
        private readonly IAuthService _authService = authService;

        /// <summary>
        /// IUser
        /// </summary>
        private readonly IUser _userService = userService;

        /// <summary>
        /// IAccessTokenProviderAccessor
        /// </summary>
        private readonly IAccessTokenProviderAccessor _accessor = accessor;

        /// <summary>
        /// IMicrosoftCalendarService
        /// </summary>
        private readonly IMicrosoftCalendarService _microsoftCalendarService = microsoftCalendarService;

        /// <summary>
        /// CreateUserAsync
        /// </summary>
        /// <param name="account"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override async ValueTask<ClaimsPrincipal> CreateUserAsync(CustomUserAccount account, RemoteAuthenticationUserOptions options)
        {
            var initialUser = await base.CreateUserAsync(account, options);

            if (initialUser.Identity is not null && initialUser.Identity.IsAuthenticated)
            {
                var graphTokenResult = await _accessor.TokenProvider.RequestAccessToken(new AccessTokenRequestOptions
                {
                    Scopes = new[] { "https://graph.microsoft.com/Calendars.ReadWrite" }
                });
                if (graphTokenResult.TryGetToken(out var graphToken))
                {
                    if (graphToken is not null)
                    {
                        ExternalLoginDTO externalLoginDTO = new ExternalLoginDTO()
                        {
                            FullName = initialUser.Claims.First(c => c.Type == "name").Value!,
                            Email = initialUser.Claims.First(c => c.Type == "preferred_username").Value!,
                            GraphToken = graphToken.Value
                        };
                        GenerateTokenResponse response = await _userService.ExternalLoginAsync(externalLoginDTO)
                            .ConfigureAwait(false);
                        await _authService.UpdateAuthenticationState(response.Token, graphToken.Value).ConfigureAwait(false);
                    }
                }
            }
            return initialUser;
        }
    }
}
