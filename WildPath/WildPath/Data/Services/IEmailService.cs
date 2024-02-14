using SendGrid;
using WildPath.Models;

namespace SendGridDemo.Data.Services
{
    public interface IEmailService
    {
        Task<Response> SendSingleEmail(ComposeEmailModel payload);
    }
}
