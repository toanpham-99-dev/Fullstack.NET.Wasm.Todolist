using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Hangfire.PostgreSql;
using WorkManagermentWeb.Background.Sevices;

namespace WorkManagermentWeb.Background.DependencyInjection
{
    /// <summary>
    /// ServiceContainer
    /// </summary>
    public static class ServiceContainer
    {
        /// <summary>
        /// BackgroundJobServices
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IServiceCollection BackgroundJobServices(this IServiceCollection services, string connectionString)
        {
            services.AddHangfire(config =>
                config.UsePostgreSqlStorage(c =>
                    c.UseNpgsqlConnection(connectionString)));
            services.AddHangfireServer();
            services.AddScoped<IBackground, BackgroundService>();
            return services;
        }
    }
}
