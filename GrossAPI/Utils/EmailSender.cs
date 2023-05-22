using Azure.Core;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using System.Web;

namespace GrossAPI.Utils
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        public EmailSettings _emailSettings { get; set; }
        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

        public async Task Execute(string email, string subject, string htmlMessage)
        {
            try
            {
                _emailSettings = _configuration.GetSection("Email").Get<EmailSettings>();

                MimeMessage message = new MimeMessage();

                message.From.Add(new MailboxAddress("Бухгалтерия Гросс", _emailSettings.Login)); 
                message.To.Add(new MailboxAddress("Client", email));
                message.Subject = subject; 
                message.Body = new BodyBuilder { HtmlBody = htmlMessage }.ToMessageBody();

                using (MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient())
                {
                   await client.ConnectAsync("smtp.gmail.com", 465, true); //либо порт 465
                   await client.AuthenticateAsync(_emailSettings.Login, _emailSettings.Password); 
                   await client.SendAsync(message);
                   await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {

            }
        }      
    }
}
