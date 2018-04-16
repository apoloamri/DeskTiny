using System.IO;
using System.Net;
using System.Net.Mail;
using Tenderfoot.Database;
using Tenderfoot.TfSystem;

namespace Tenderfoot.Net
{
    public static class TfEmail
    {
        public static void Send(string mailTo, string emailFile, params string[] items)
        {
            var emailContent = ReadEmailFromFile(emailFile, items);
            SendBase(mailTo, emailContent.Title, emailContent.Message);
        }

        private static void SendBase(string mailTo, string title, string message)
        {
            var mailFrom = Settings.Web.SmtpEmail;
            var mail = new MailMessage(mailFrom, mailTo) { IsBodyHtml = true };

            var client = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = true,
                Host = Settings.Web.SmtpHost,
                Port = Settings.Web.SmtpPort,
                Credentials = new NetworkCredential(mailFrom, Settings.Web.SmtpPassword)
            };

            mail.Subject = title;
            mail.Body = message;

            client.Send(mail);

            var emails = Schemas.Emails;
            emails.Entity.mail_from = mailFrom;
            emails.Entity.mail_to = mailTo;
            emails.Entity.mail_title = title;
            emails.Entity.mail_body = message;
            emails.Insert();
        }

        private static EmailContent ReadEmailFromFile(string fileName, string[] items)
        {
            var message = File.ReadAllText(Path.Combine(Settings.SystemResources.EmailFiles, fileName + ".html"));
            message = message.Replace("{url}", Settings.Web.SiteUrl);
            message = message.Replace("{api_url}", Settings.Web.ApiUrl);
            message = string.Format(message, items);
            
            return new EmailContent()
            {
                Title = fileName,
                Message = message
            };
        }

        private class EmailContent
        {
            public string Title { get; set; }
            public string Message { get; set; }
        }
    }
}
