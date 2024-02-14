using SendGrid;
using WildPath.Models;

namespace WildPath.Data.Services
{
    public interface IEmailService
    {
        Task<Response> SendSingleEmail(ComposeEmailModel payload);
    }

}
