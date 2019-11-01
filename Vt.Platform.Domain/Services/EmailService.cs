using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Vt.Platform.Domain.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }
        public async Task SendEmail(MailAddress[] to, string subject, string body)
        {
            var email = Environment.GetEnvironmentVariable("Smtp.FromEmail");
            var name = Environment.GetEnvironmentVariable("Smtp.FromName");

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(name))
            {
                _logger.LogError("Cannot send email as sender email/name is not present");
                return;
            }

            string smtpUsername = Environment.GetEnvironmentVariable("Smtp.Username");
            string smtpPassword = Environment.GetEnvironmentVariable("Smtp.Password"); ;
            string host = Environment.GetEnvironmentVariable("Smtp.Host");
            int port = Convert.ToInt32(Environment.GetEnvironmentVariable("Smtp.Port"));

            // Create and build a new MailMessage object
            MailMessage message = new MailMessage
            {
                IsBodyHtml = true,
                From = new MailAddress(email, name)
            };

            foreach (var address in to)
            {
                message.To.Add(address);
            }

            message.Subject = subject;
            message.Body = body;

            using (var client = new SmtpClient(host, port))
            {
                // Pass SMTP credentials
                client.Credentials =
                    new NetworkCredential(smtpUsername, smtpPassword);

                // Enable SSL encryption
                client.EnableSsl = true;

                // Try to send the message. Show status in console.
                try
                {
                    await client.SendMailAsync(message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"The email was not sent.");
                }
            }
        }
    }
}
