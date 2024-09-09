using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Twilio.Types;
using Twilio;
using WorkManagermentWeb.Infrastructure.Options;
using Twilio.Rest.Api.V2010.Account;

namespace WorkManagermentWeb.Infrastructure.Services
{
    /// <summary>
    /// SMSSenderService
    /// </summary>
    public class SmsSenderService : ISmsSender
    {
        /// <summary>
        /// SmsOptions
        /// </summary>
        private readonly SmsOptions _options;

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<SmsSenderService> _logger;

        /// <summary>
        /// ApiEmailSender
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public SmsSenderService(IOptions<SmsOptions> options, ILogger<SmsSenderService> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        /// <summary>
        /// SendAsync
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task SendAsync(string phone, string message)
        {
            var accountSid = _options.SMSAccountIdentification;
            var authToken = _options.SMSAccountPassword;

            TwilioClient.Init(accountSid, authToken);

            MessageResource result = default!;

            try
            {
                result = await MessageResource.CreateAsync(
                    to: new PhoneNumber(phone),
                    from: new PhoneNumber(_options.SMSAccountFrom),
                    body: message);
            }
            catch (Exception ex)
            {

                _logger.LogError($"{nameof(SmsSenderService)}: {ex.Message}");
            }
        }
    }
}
