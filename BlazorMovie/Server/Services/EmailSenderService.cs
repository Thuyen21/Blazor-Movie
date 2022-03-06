using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid.Helpers.Mail;

namespace BlazorMovie.Server.Services
{
    public class EmailSenderService : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("Joe@contoso.com", "Password Recovery"),
                Subject = subject,
                PlainTextContent = htmlMessage,
                HtmlContent = htmlMessage
            };
            msg.AddTo(new EmailAddress(email));
        }
    }
}
