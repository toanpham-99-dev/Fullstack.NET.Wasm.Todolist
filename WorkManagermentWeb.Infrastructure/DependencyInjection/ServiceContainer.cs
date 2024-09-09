using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkManagermentWeb.Infrastructure.Data;
using WorkManagermentWeb.Application.Contracts;
using WorkManagermentWeb.Infrastructure.Repositories;
using WorkManagermentWeb.Authorization.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using WorkManagermentWeb.Domain.Entities;
using WorkManagermentWeb.Infrastructure.Options;
using Microsoft.AspNetCore.Identity.UI.Services;
using WorkManagermentWeb.Infrastructure.Services;
using WorkManagermentWeb.Infrastructure.Jobs;
using WorkManagermentWeb.Pusher.Options;
using WorkManagermentWeb.Pusher.Services;
using WorkManagermentWeb.EventHandler.DependencyInjection;

namespace WorkManagermentWeb.Infrastructure.DependencyInjection
{
    /// <summary>
    /// ServiceContainer
    /// </summary>
    public static class ServiceContainer
    {
        /// <summary>
        /// InfrastructureServices
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection InfrastructureServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<CommonOptions>(
                configuration.GetSection(nameof(CommonOptions)));
            services.Configure<MicrosoftGraphOptions>(
                configuration.GetSection(nameof(MicrosoftGraphOptions)));
            services.AddDbContext<AppDBContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ServiceContainer).Assembly.FullName)),
                ServiceLifetime.Scoped);
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            //Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDBContext>()
                .AddDefaultTokenProviders()
                .AddSignInManager()
                .AddRoles<IdentityRole>();

            //Email
            services.AddEmailService(configuration);

            //Sms
            services.AddSmsService(configuration);

            //Add Auth
            services.AuthorizationServices(configuration);

            //Add meditR
            services.AddMediatRNotification();

            //Add pusher
            services.Configure<PusherOptions>(configuration.GetSection(nameof(PusherOptions)));
            services.AddTransient<ISendMessageService, SendMessageService<AppDBContext, Notification, NotificationLanguage>>();

            services.AddHttpClient<MicrosoftCalendarService>();

            services.AddScoped<IRemindNearingDuesJob, RemindNearingDuesJob>()
                .AddScoped<IWorkSpace, WorkSpaceRepository>()
                .AddScoped<IBoard, BoardRepsitory>()
                .AddScoped<IUser, UserRepository>()
                .AddScoped<IWorkItem, WorkItemRepository>()
                .AddScoped<INotification, NotificationRepository>()
                .AddScoped<IGanttChart, GanttChartRepository>()
                .AddScoped<ICalendar, CalendarRepository>()
                .AddScoped<IMicrosoftCalendarService, MicrosoftCalendarService>();

            return services;
        }

        /// <summary>
        /// AddEmailService
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailOptions>(
                configuration.GetSection(nameof(EmailOptions)));
            services.AddScoped<IEmailSender, ApiEmailSender>();
            return services;
        }

        /// <summary>
        /// AddSmsService
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddSmsService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SmsOptions>(
                configuration.GetSection(nameof(SmsOptions)));
            services.AddScoped<ISmsSender, SmsSenderService>();
            return services;
        }
    }
}
