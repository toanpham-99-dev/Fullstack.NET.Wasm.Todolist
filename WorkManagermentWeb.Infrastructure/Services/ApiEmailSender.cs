using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;
using WorkManagermentWeb.Application.Constants;
using WorkManagermentWeb.Application.DTOs.Requests;
using WorkManagermentWeb.Infrastructure.Options;

namespace WorkManagermentWeb.Infrastructure.Services
{
    /// <summary>
    /// ApiEmailSender
    /// </summary>
    public class ApiEmailSender : IEmailSender
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
        /// ApiEmailSender
        /// </summary>
        /// <param name="emailOptions"></param>
        /// <param name="logger"></param>
        public ApiEmailSender(IOptions<EmailOptions> emailOptions, ILogger<SmtpEmailSender> logger)
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
        /// <exception cref="NotImplementedException"></exception>
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using var client = new RestClient(_emailOptions.ApiUrl);
            var request = new RestRequest();

            ApiEmailRequest bodyRequest = new ApiEmailRequest()
            {
                Subject = subject,
                Html = htmlMessage,
                From = new From { Email = _emailOptions.Mail, Name = _emailOptions.DisplayName },
                To = new List<From>() { new From { Email = email } }
            };

            request.AddHeader(AuthorizationConstants.AuthorizationSectionName, $"{AuthorizationConstants.Bearer} {_emailOptions.ApiToken}");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", bodyRequest, ParameterType.RequestBody);
            var response = await client.PostAsync(request);
            System.Console.WriteLine(response.Content);
        }
    }
}
