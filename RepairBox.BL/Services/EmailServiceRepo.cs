using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace RepairBox.BL.Services
{
    public interface IEmailServiceRepo
    {
        Task SendEmailAsync(string recipient, string subject, string body);
        Task SendBulkEmailAsync(List<string> recipients, string subject, string body);
    }
    public class EmailServiceRepo : IEmailServiceRepo
    {
        private IConfiguration _config;
        public EmailServiceRepo(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendBulkEmailAsync(List<string> recipients, string subject, string body)
        {
            try
            {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_config.GetSection("MailSettings:Mail").Value);
                foreach (var recipient in recipients)
                    email.To.Add(MailboxAddress.Parse(recipient));
                email.Subject = subject;
                var builder = new BodyBuilder();
                builder.HtmlBody = body;
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(_config.GetSection("MailSettings:Host").Value, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_config.GetSection("MailSettings:Mail").Value, _config.GetSection("MailSettings:Password").Value);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SendEmailAsync(string recipient, string subject, string body)
        {
            try
            {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_config.GetSection("MailSettings:Mail").Value);
                email.To.Add(MailboxAddress.Parse(recipient));
                email.Subject = subject;
                var builder = new BodyBuilder();
                builder.HtmlBody = body;
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(_config.GetSection("MailSettings:Host").Value, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_config.GetSection("MailSettings:Mail").Value, _config.GetSection("MailSettings:Password").Value);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
