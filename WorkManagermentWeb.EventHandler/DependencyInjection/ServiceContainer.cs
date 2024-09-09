using Microsoft.Extensions.DependencyInjection;
using WorkManagermentWeb.EventHandler.Services;
using MediatR;

namespace WorkManagermentWeb.EventHandler.DependencyInjection
{
    /// <summary>
    /// ServiceContainer
    /// </summary>
    public static class ServiceContainer
    {
        /// <summary>
        /// AddMediatRNotification
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMediatRNotification(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ServiceContainer));

            services.AddTransient<IEventPusher, MediatREventPusher>();

            return services;
        }
    }
}
