using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace Utils
{
    public class EmailManager : IEmailManager
    {
        public async void SendEmail(Email email)
        {
            var smtp = new SmtpClient
            {
                Host = email.SmtpAddress,
                Port = email.Port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(email.From, email.Password)
            };

            using (var message = new MailMessage(email.From, email.To))
            {
                message.Subject = email.Header;
                message.Body = email.Body;

                try
                {
                    await smtp.SendMailAsync(message);
                }
                catch(Exception e)
                {
                    throw e;
                }
            }
        }
    }
}
