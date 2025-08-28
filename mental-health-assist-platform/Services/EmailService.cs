using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;
using mental_health_assist_platform.DTO;
using mental_health_assist_platform.Services;
using mental_health_assist_platform.Configuration;

namespace mental_health_assist_platform.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailRequest request);
    }
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(EmailRequest request)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_emailSettings.SenderEmail);
            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            email.Subject = request.Subject;

            var builder = new BodyBuilder { HtmlBody = request.Body };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(
                _emailSettings.SmtpServer,
                _emailSettings.Port,
                MailKit.Security.SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
