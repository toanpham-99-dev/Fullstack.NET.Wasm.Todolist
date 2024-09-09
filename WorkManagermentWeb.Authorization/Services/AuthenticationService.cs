using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WorkManagermentWeb.Application.DTOs;
using WorkManagermentWeb.Authorization.Options;

namespace WorkManagermentWeb.Authorization.Services
{
    /// <summary>
    /// AuthenticationService
    /// </summary>
    public class AuthenticationService : IAuthentication
    {
        /// <summary>
        /// JwtAuthenticationOptions
        /// </summary>
        private readonly JwtAuthenticationOptions _options;

        /// <summary>
        /// AuthenticationService
        /// </summary>
        /// <param name="options"></param>
        public AuthenticationService(IOptionsMonitor<JwtAuthenticationOptions> options)
        {
            _options = options.CurrentValue;
        }

        /// <summary>
        /// GenerateJwtToken
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string GenerateJwtToken(UserSession user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id!),
                new Claim(ClaimTypes.Name, user.Name!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, user.Role!)
            };
            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: userClaims,
                expires: DateTime.Now.AddDays(5),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
