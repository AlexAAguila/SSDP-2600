using Azure;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;
using WildPath.Models;

namespace WildPath.Data.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<SendGrid.Response> SendSingleEmail(ComposeEmailModel payload)
        {
            var apiKey = _configuration.GetSection("SendGrid")["ApiKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("craig_watson@bcit.ca", "Craig Watson");
            var subject = payload.Subject;
            var to = new EmailAddress(payload.Email
                                     , $"{payload.FirstName} {payload.LastName}");
            var textContent = payload.Body;
            var htmlContent = $"<strong>{payload.Body}</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject
                                                    , textContent, htmlContent);
            var request = client.SendEmailAsync(msg);
            request.Wait();
            var result = request.Result;
            return request;
        }
    }

}
