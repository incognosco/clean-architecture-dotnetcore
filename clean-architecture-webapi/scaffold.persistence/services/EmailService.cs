using Scaffold.Application.Exceptions;
using Scaffold.Application.Interfaces.Services;
using Scaffold.Domain.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Text.Json;
using System.Threading.Tasks;

namespace Scaffold.Persistence.Services
{
    public class EmailService : IEmailService
    {
        public MailSettings _mailSettings { get; }
        public ILogger<EmailService> _logger { get; }

        public EmailService(IOptions<MailSettings> mailSettings, ILogger<EmailService> logger, ITenantService tenant)
        {
            var options = new System.Text.Json.JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            };
            TenantSiteConfiguration config = System.Text.Json.JsonSerializer.Deserialize<TenantSiteConfiguration>(tenant.GetSiteConfiguration(), options);
            _mailSettings = config.smtp;
            _logger = logger;
        }

        public async Task SendAsync(EmailRequest request)
        {
            try
            {
                // create message
                var message = new System.Text.StringBuilder();
                var fromAddress = string.Empty;
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_mailSettings.EmailFrom);

                if (request.Subject.ToLower() == "data")
                {
                    foreach (var item in _mailSettings.DataEmail.Split(","))
                    {
                        email.To.Add(MailboxAddress.Parse(request.To ?? item));
                    }                    
                } else
                {
                    foreach (var item in _mailSettings.TechnicalEmail.Split(","))
                    {
                        email.To.Add(MailboxAddress.Parse(request.To ?? item));
                    }
                }
                
                email.Subject = (request.Subject.ToLower() == "data" ?
                        _mailSettings.DataSubject:
                        _mailSettings.TechnicalSubject);
                
                var builder = new BodyBuilder();
                builder.HtmlBody = request.Body;
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                email.From.Add(MailboxAddress.Parse(_mailSettings.EmailFrom));
                smtp.Connect(_mailSettings.SmtpHost, _mailSettings.SmtpPort, options: SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.SmtpUser, _mailSettings.SmtpPass);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);

            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new ApiException(ex.Message);
            }
        }

        class TenantSiteConfiguration
        {
            public MailSettings smtp { get; set; }
        }

    }
}
