using System.Net.Mail;
using System.Threading.Tasks;

namespace Vt.Platform.Domain.Services
{
    public interface IEmailService
    {
        Task SendEmail(MailAddress[] to, string subject, string body);
    }
}