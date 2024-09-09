using WorkManagermentWeb.Application.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Text.Json.Serialization;
using ClaimsPrincipal = System.Security.Claims.ClaimsPrincipal;
using ClaimsIdentity = System.Security.Claims.ClaimsIdentity;
using Claim = System.Security.Claims.Claim;
using ClaimTypes = System.Security.Claims.ClaimTypes;
using WorkManagermentWeb.Application.Constants;

namespace WorkManagermentWeb.Application.GenericModels
{
    /// <summary>
    /// Generics
    /// </summary>
    public static class Generics
    {
        /// <summary>
        /// SetClaimPrincipal
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ClaimsPrincipal SetClaimPrincipal(UserSession model)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(
                new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, model.Id!),
                    new(ClaimTypes.Name, model.Name!),
                    new(ClaimTypes.Email, model.Email!),
                    new(ClaimTypes.Role, model.Role!),
                }, AuthenticationTypes.Password));
        }

        /// <summary>
        /// GetClaimsFromToken
        /// </summary>
        /// <param name="jwtToken"></param>
        /// <returns></returns>
        public static UserSession GetClaimsFromToken(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);
            var claims = token.Claims;

            string Id = claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value!;
            string Name = claims.First(c => c.Type == ClaimTypes.Name).Value!;
            string Email = claims.First(c => c.Type == ClaimTypes.Email).Value!;
            string Role = claims.First(c => c.Type == ClaimTypes.Role).Value!;

            return new UserSession(Id, Name, Email, Role);
        }

        /// <summary>
        /// JsonSerializerOptions
        /// </summary>
        /// <returns></returns>
        public static JsonSerializerOptions JsonOptions()
        {
            return new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip
            };
        }

        /// <summary>
        /// SerializeObj
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="modelObject"></param>
        /// <returns></returns>
        public static string SerializeObj<T>(T modelObject) => JsonSerializer.Serialize(modelObject, JsonOptions());

        /// <summary>
        /// DeserializeJsonString
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T DeserializeJsonString<T>(string jsonString) => JsonSerializer.Deserialize<T>(jsonString, JsonOptions())!;

        /// <summary>
        /// DeserializeJsonStringList
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static IList<T> DeserializeJsonStringList<T>(string jsonString) => JsonSerializer.Deserialize<IList<T>>(jsonString, JsonOptions())!;

        /// <summary>
        /// GenerateStringContent
        /// </summary>
        /// <param name="serialiazedObj"></param>
        /// <returns></returns>
        public static StringContent GenerateStringContent(string serialiazedObj) => new(serialiazedObj, System.Text.Encoding.UTF8, "application/json");
    }
}
