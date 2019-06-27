using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;


namespace ClanWeb.Core.Comunication
{
    public class EmailService : IEmailService, IIdentityMessageService, IDisposable
    {
        public SmtpClient SmtpServer { get; private set; }


        /// <summary>
        /// Creates instacne of the email service and sends a message when needed
        /// </summary>
        /// <param name="server"></param>
        public EmailService(SmtpClient server)
        {
            // Checking if there was a acctual smtp client added
            if (server == null)
            {
                throw new ArgumentNullException(nameof(server));
            }

            // Set the client
            this.SmtpServer = server;
        }

        /// <summary>
        /// Sets a email service from
        /// </summary>
        public EmailService()
        {
            // Setting up a new smtp client
            this.SmtpServer = new SmtpClient();
        }


        /// <summary>
        /// Sends a message with smtp
        /// </summary>
        /// <param name="message">The message that want to be send</param>
        public virtual async Task SendAsync(MailMessage message)
        {
            // Making sure that the message is not nothing
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            try
            {
                // Sending the message
                await SmtpServer.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Sends a message with smtp
        /// </summary>
        /// <param name="message">The message that want to be send</param>
        public virtual async Task SendAsync(IdentityMessage message)
        {
            // Making sure that the message is not nothing
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            // Converting identity message to a message
            MailMessage result = new MailMessage()
            {
                IsBodyHtml = true,
                Subject = message.Subject,
                Body = message.Body,
            };

            // Setting the reciving email address
            result.To.Add(message.Destination);

            try
            {
                // Sending the message
                await SmtpServer.SendMailAsync(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public virtual void Dispose()
        {
            if (SmtpServer != null)
            {
                SmtpServer.Dispose();
                SmtpServer = null;
            }
        }

      
    }
}
