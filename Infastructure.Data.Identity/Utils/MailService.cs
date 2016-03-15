using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace FashionStore.Infrastructure.Data.Identity.Utils
{
    public class MailService : IIdentityMessageService
    {
        private readonly string _smtpServer;
        private string _smtpRequiresAuthentication;
        private string _smtpUseSsl;
        private int _smtpPort;
        private string _smtpUser;
        private string _smtpPassword;
        private int _smtpTimeoutInMilliseconds;
        private string _smtpPreferredEncoding;

        public MailService()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            _smtpServer = appSettings["SMTPServer"];
            _smtpRequiresAuthentication = appSettings["SMTPRequiresAuthentication"];
            _smtpUseSsl = appSettings["SMTPUseSsl"];
            _smtpPort = Convert.ToInt32(appSettings["SMTPPort"]);
            _smtpUser = appSettings["SMTPUser"];
            _smtpPassword = appSettings["SMTPPassword"];
            _smtpTimeoutInMilliseconds = Convert.ToInt32(appSettings["SMTPTimeoutInMilliseconds"]);
            _smtpPreferredEncoding = appSettings["SmtpPreferredEncoding"];

        }

        public Task SendAsync(IdentityMessage message)
        {
            SmtpClient client = new SmtpClient();
            client.Host = _smtpServer;
            client.Port = _smtpPort;
            client.EnableSsl = Convert.ToBoolean(_smtpUseSsl);
            client.Credentials = new NetworkCredential(_smtpUser, _smtpPassword);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(_smtpUser);
            mail.To.Add(new MailAddress(message.Destination));
            mail.Subject = message.Subject;
            mail.Body = message.Body;
            mail.IsBodyHtml = true;

            return client.SendMailAsync(mail);

        }
    }
}