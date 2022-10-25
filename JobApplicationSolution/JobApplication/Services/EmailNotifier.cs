using System.Text;
using System.Net;
using System.Net.Mail;

using JobApplication.Interfaces;
using JobApplication.Configurations;

namespace JobApplication.Services
{
    public class EmailNotifier : INotifier
    {
        private EmailConfiguration emailConfiguration = new();

        private MailMessage mailMessage;

        public EmailNotifier() => InitializeEmailMessage();

        public void Notify(string message)
        {
            mailMessage.Body = message;

            SendEmail();
        }

        private void InitializeEmailMessage()
        {
            var sender = new MailAddress(emailConfiguration.Sender);

            mailMessage = new MailMessage() { From = sender };

            mailMessage.Subject = emailConfiguration.Subject;

            mailMessage.IsBodyHtml = false;
            mailMessage.SubjectEncoding = Encoding.UTF8;
            mailMessage.BodyEncoding = Encoding.UTF8;

            foreach (var recipient in emailConfiguration.Recipients)
                mailMessage.To.Add(recipient);
        }
        private async void SendEmail()
        {
            using var client = new SmtpClient();

            client.EnableSsl = true;
            client.Host = emailConfiguration.Host;
            client.Port = emailConfiguration.Port;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential(emailConfiguration.Sender, emailConfiguration.Password);

            try
            {
                Logger.Log("Trying to send out e-mail notification ...");

                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Logger.Log($"Some error occurred while trying to send out the notification emails . Details: {ex.Message}");
            }
        }

    }
}