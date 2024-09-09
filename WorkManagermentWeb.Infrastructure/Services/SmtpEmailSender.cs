using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using WorkManagermentWeb.Infrastructure.Options;

namespace WorkManagermentWeb.Infrastructure.Services
{
    /// <summary>
    /// EmailSenderService
    /// </summary>
    public class SmtpEmailSender : IEmailSender
    {
        /// <summary>
        /// EmailOptions
        /// </summary>
        private readonly EmailOptions _emailOptions;

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<SmtpEmailSender> _logger;

        /// <summary>
        /// EmailSenderService
        /// </summary>
        /// <param name="emailOptions"></param>
        /// <param name="logger"></param>
        public SmtpEmailSender(IOptions<EmailOptions> emailOptions, ILogger<SmtpEmailSender> logger)
        {
            _emailOptions = emailOptions.Value;
            _logger = logger;
        }

        /// <summary>
        /// SendEmailAsync
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="htmlMessage"></param>
        /// <returns></returns>
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage();
            message.Sender = new MailboxAddress(_emailOptions.DisplayName, _emailOptions.Mail);
            message.From.Add(new MailboxAddress(_emailOptions.DisplayName, _emailOptions.Mail));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = htmlMessage;
            message.Body = builder.ToMessageBody();

            // Use MailKit SmtpClient
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                smtp.Connect(_emailOptions.Host, _emailOptions.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_emailOptions.Mail, _emailOptions.Password);
                await smtp.SendAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(SmtpEmailSender)}: {ex.Message}");
            }

            smtp.Disconnect(true);
        }
    }
}
