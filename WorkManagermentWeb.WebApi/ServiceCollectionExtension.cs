using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using WorkManagermentWeb.Application.Constants;

namespace WorkManagermentWeb.WebApi
{
    /// <summary>
    /// ServiceCollectionExtension
    /// </summary>
    internal static class ServiceCollectionExtension
    {
        /// <summary>
        /// AddSwagger
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        internal static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            OpenApiSecurityScheme jwtSecurityScheme = new()
            {
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                BearerFormat = AuthorizationConstants.Jwt,
                Name = AuthorizationConstants.BearerName,
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WorkManagermentWeb", Version = "v1" });
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme,
                    new OpenApiSecurityScheme
                    {
                        Description = AuthorizationConstants.AuthorizeDescription,
                        Name = AuthorizationConstants.AuthorizationSectionName,
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = JwtBearerDefaults.AuthenticationScheme
                    });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement { { jwtSecurityScheme, Array.Empty<string>() } });
            });
            return services;
        }
    }
}
