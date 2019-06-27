using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TestingApplication
{
    class Program
    {

        public static string GenerateCode(int lenght = 255)
        {
            Random random = new Random();
            char[] theRandomChar = "abcdefghijklmnopqrstuvwyxzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
            string theRandomFinalString = "";

            for (int i = 0; i < lenght; i++)
            {
                char randomChar = theRandomChar[random.Next(0, theRandomChar.Length)];
                theRandomFinalString += randomChar;
            }

            return theRandomFinalString;

        }


        static void Main(string[] args)
        {

            EmailManager manager = new EmailManager();

            manager.SendEmailConfermation("kdteuling@yahoo.com", "Hello world");

        }
    }






    public class EmailManager
    {
        private SmtpClient smtpClient;

        private MailAddress _fromEmailAddress;
        private FromEmailAddress _from;
        private MailAddress _to;
        private string _subject;
        private string _body;


        /// <summary>
        /// Email comeing from
        /// </summary>
        public FromEmailAddress From
        {
            get { return _from; }
            set { _from = value; }
        }


        public MailAddress FromMailAddress
        {
            get { return _fromEmailAddress; }
            set { _fromEmailAddress = value; }
        }


        /// <summary>
        /// Email going to
        /// </summary>
        public MailAddress To
        {
            get { return _to; }
            set { _to = value; }
        }

        /// <summary>
        /// Subject of the email
        /// </summary>
        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        /// <summary>
        /// Body of the email
        /// </summary>
        public string Body
        {
            get { return _body; }
            set { _body = value; }
        }



        /// <summary>
        /// Email Manager that handels all the email funconality of the application
        /// </summary>
        public EmailManager()
        {
            // Setting the smtp info
            smtpClient = new SmtpClient("smtp.zoho.eu", 587);

            // Setting the no reply email address
            smtpClient.Credentials = new NetworkCredential("NoReply@initsquad.com", "kevindt12");
            FromMailAddress = new MailAddress("noreply@initsquad.com", "Init Squad");

            // Setting Ssl
            smtpClient.EnableSsl = true;
        }

        /// <summary>
        /// Email Manager that handels all the email funconality of the application
        /// </summary>
        /// <param name="fromEmailAdress">From where sould the email come from</param>
        public EmailManager(FromEmailAddress fromEmailAdress = FromEmailAddress.NoReply)
        {
            // Setting the smtp info
            smtpClient = new SmtpClient("smtp.zoho.eu", 587);

            // Seleting from wich email it sould come from
            switch (fromEmailAdress)
            {
                case FromEmailAddress.NoReply:
                smtpClient.Credentials = new NetworkCredential("NoReply@initsquad.com", "kevindt12");
                FromMailAddress = new MailAddress("noreply@initsquad.com", "Init Squad");
                break;
                case FromEmailAddress.Admins:
                throw new NotImplementedException();
                case FromEmailAddress.Kevin:
                throw new NotImplementedException();
            }

            // Setting Ssl
            smtpClient.EnableSsl = true;
        }

        /// <summary>
        /// Email Manager that handels all the email funconality of the application
        /// </summary>
        /// <param name="smptServer">Custom smtp Server</param>
        /// <param name="port">Custom port</param>
        public EmailManager(string smptServer = "smtp.zoho.eu", int port = 587, bool enableSsl = true, FromEmailAddress fromEmailAdress = FromEmailAddress.NoReply)
        {
            // Setting the smtp server settings
            smtpClient = new SmtpClient(smptServer, port);

            // Seleting from wich email it sould come from
            switch (fromEmailAdress)
            {
                case FromEmailAddress.NoReply:
                smtpClient.Credentials = new NetworkCredential("NoReply@initsquad.com", "kevindt12");
                FromMailAddress = new MailAddress("noreply@initsquad.com", "Init Squad");
                break;
                case FromEmailAddress.Admins:
                throw new NotImplementedException();
                case FromEmailAddress.Kevin:
                throw new NotImplementedException();
            }

            // Setting the ssl settings
            smtpClient.EnableSsl = enableSsl;


        }



        public void SendEmailConfermation(string recipiant, string callBackUrl)
        {
            // Setting a new email address for the recver
            MailAddress reciver = new MailAddress(recipiant);

            // Creating a new Email message
            MailMessage email = new MailMessage();

            email.IsBodyHtml = true;
            email.To.Add(reciver);
            email.From = FromMailAddress;
            email.Subject = "Email confomation link";
            email.Body = "Please Fonfurm your email by pressing this link: <a href=\"" + callBackUrl + "\">link</a>";

            smtpClient.Send(email);
        }

        public void SendPasswordRecovery()
        {

        }

        public void SendGenericEmail()
        {

        }

        public void SendBulkEmail()
        {

        }






    }


    public enum FromEmailAddress
    {
        NoReply,
        Admins,
        Kevin
    }
}
