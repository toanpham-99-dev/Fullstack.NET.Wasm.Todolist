using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WorkManagermentWeb.Application.Constants;
using WorkManagermentWeb.Authorization.Options;
using AuthService = WorkManagermentWeb.Authorization.Services.AuthenticationService;
using IAuthService = WorkManagermentWeb.Authorization.Services.IAuthentication;

namespace WorkManagermentWeb.Authorization.DependencyInjection
{
    /// <summary>
    /// ServiceContainer
    /// </summary>
    public static class ServiceContainer
    {
        /// <summary>
        /// AuthorizationServices
        /// </summary>
        /// <param name = "services" ></ param >
        /// < param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AuthorizationServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<JwtAuthenticationOptions>(
                configuration.GetSection(AuthorizationConstants.Jwt));
            JwtAuthenticationOptions jwtOptions =
                configuration.GetSection(AuthorizationConstants.Jwt).Get<JwtAuthenticationOptions>()!;

#pragma warning disable CS0618 // Type or member is obsolete
            services.AddAuthentication(AzureADDefaults.BearerAuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                    ClockSkew = TimeSpan.Zero
                };
            })
            .AddMicrosoftIdentityWebApi(configuration.GetSection("AzureAd"), AzureADDefaults.BearerAuthenticationScheme)
            .EnableTokenAcquisitionToCallDownstreamApi()
            .AddInMemoryTokenCaches();
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizationConstants.TwoAuthPolicy, policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.AuthenticationSchemes.Add(AzureADDefaults.BearerAuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                });
            });
#pragma warning restore CS0618 // Type or member is obsolete

            services.AddScoped<IAuthService, AuthService>();
            return services;
        }
    }
}
