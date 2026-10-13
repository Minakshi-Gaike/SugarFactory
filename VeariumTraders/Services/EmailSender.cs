
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace VeariumTraders.Services
{
    public class EmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(
            string toEmail,
            string subject,
            string htmlMessage)
        {
            var host = _configuration["EmailSettings:Host"];
            var port = int.Parse(
                _configuration["EmailSettings:Port"] ?? "587");
            var username = _configuration["EmailSettings:Username"];
            var senderEmail =
                _configuration["EmailSettings:SenderEmail"];
            var appPassword =
                _configuration["EmailSettings:AppPassword"];

            if (string.IsNullOrWhiteSpace(host) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(senderEmail) ||
                string.IsNullOrWhiteSpace(appPassword))
            {
                throw new InvalidOperationException(
                    "Email settings are incomplete.");
            }

            var message = new MimeMessage();

            message.From.Add(
                new MailboxAddress("Vearium Traders", senderEmail));

            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;

            message.Body = new BodyBuilder
            {
                HtmlBody = htmlMessage
            }.ToMessageBody();

            using var client = new SmtpClient();

            await client.ConnectAsync(
                host,
                port,
                SecureSocketOptions.StartTls);

            await client.AuthenticateAsync(
                username,
                appPassword);

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
