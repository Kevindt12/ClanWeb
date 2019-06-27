using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace ClanWeb.Core.Comunication
{
    public interface IEmailService
    {

        /// <summary>
        /// Sends a email using a default mail message
        /// </summary>
        /// <param name="message">The message you want to send</param>
        Task SendAsync(MailMessage message);

        /// <summary>
        /// Sends a mail message with a identity message
        /// </summary>
        /// <param name="message">The message you would like to send</param>
        Task SendAsync(IdentityMessage message);
    }
}
