using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using WorkManagermentWeb.Pusher.IEntities;
using WorkManagermentWeb.Pusher.Models;
using SDKPusher = PusherServer.Pusher;
using PusherOptions = WorkManagermentWeb.Pusher.Options.PusherOptions;
using WorkManagermentWeb.Pusher.Constants;

namespace WorkManagermentWeb.Pusher.Services
{
    public class SendMessageService<TDbContext, TNotification, TNotificationLanguage> : ISendMessageService
        where TDbContext : DbContext
        where TNotification : class, INotification, new()
        where TNotificationLanguage : class, INotificationLanguage, new()
    {
        /// <summary>
        ///     PusherOptions
        /// </summary>
        private readonly PusherOptions _pusherOptions;

        /// <summary>
        ///     DbContext
        /// </summary>
        private DbContext _dbContext;

        /// <summary>
        ///     ILogger
        /// </summary>
        private ILogger _logger;

        /// <summary>
        ///     SendMessageService
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="dbContext"></param>
        /// <param name="pusherOptions"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public SendMessageService(ILogger<ISendMessageService> logger, TDbContext dbContext, IOptionsMonitor<PusherOptions> pusherOptions)
        {
            _logger = logger;
            _dbContext = dbContext;
            _pusherOptions = pusherOptions.CurrentValue;
        }

        /// <summary>
        /// SendAsync
        /// </summary>
        /// <param name="notificationInput"></param>
        /// <returns></returns>
        public async Task<bool> SendAsync(NotificationInput notificationInput)
        {
            NotificationOutput notification = await SaveNotification(notificationInput);
            var options = new PusherServer.PusherOptions
            {
                Cluster = _pusherOptions.AppCluster,
                Encrypted = true
            };
            var pusher = new SDKPusher
            (
                _pusherOptions.AppId,
                _pusherOptions.AppKey,
                _pusherOptions.AppSecret,
                options
            );
            var result = await pusher.TriggerAsync
            (
                notificationInput.RecieverId,
                _pusherOptions.EventName,
                notification
            );
            if (result.StatusCode != HttpStatusCode.OK)
            {
                _logger.LogError("Send message fail!" + result.Body);
                return false;
            }
            return true;
        }

        /// <summary>
        /// SaveNotification
        /// </summary>
        /// <param name="notificationInput"></param>
        /// <returns></returns>
        private async Task<NotificationOutput> SaveNotification(NotificationInput notificationInput)
        {
            var newNotification = new TNotification()
            {
                Id = Guid.NewGuid(),
                RedirectAt = notificationInput.RedirectAt,
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                RecieverId = notificationInput.RecieverId
            };
            await _dbContext.Set<TNotification>().AddAsync(newNotification);

            var newNotificationLanguageVi = new TNotificationLanguage()
            {
                Id = Guid.NewGuid(),
                Lang = NotificationLang.Vietnamese,
                Content = notificationInput.VietnameseTitle,
                NotificationId = newNotification.Id
            };
            await _dbContext.Set<TNotificationLanguage>().AddAsync(newNotificationLanguageVi);

            var newNotificationLanguageEn = new TNotificationLanguage()
            {
                Id = Guid.NewGuid(),
                Lang = NotificationLang.English,
                Content = notificationInput.EnglishTitle,
                NotificationId = newNotification.Id
            };
            await _dbContext.Set<TNotificationLanguage>().AddAsync(newNotificationLanguageEn);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            notificationInput.Id = newNotification.Id;
            return new NotificationOutput(notificationInput);
        }
    }
}
