using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
// API Key - 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
namespace Lab2.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly string _sendGridKey;
        public EmailSender(IConfiguration configuration)
        {
            _sendGridKey = configuration["SendGrid:ApiKey"];
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SendGridClient(_sendGridKey);
            var from = new EmailAddress("nicole.milmine@georgebrown.ca", "Project Collaborator");
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);
            await client.SendEmailAsync(msg);
        }
    }
}
