using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Lion.Notification
{
    public class EMail
    {
        /// <summary>
        /// Send email notification
        /// </summary>
        /// <param name="sub"></param>
        /// <param name="content"></param>
        /// <param name="mailto"></param>
        /// <param name="mailfrom"></param>
        /// <param name="mailfrompassword"></param>
        public static void SendMail(string sub, string content, string mailto, string mailfrom, string mailfrompassword)
        {
            try
            {
                SmtpClient client = new SmtpClient();
                client.Port = 587;
                client.Host = "smtp.gmail.com";
                client.EnableSsl = true;
                client.Timeout = 1000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(mailfrom, mailfrompassword);

                MailMessage mail = new MailMessage(mailfrom, mailto, sub, content);
                mail.BodyEncoding = UTF8Encoding.UTF8;
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                client.Send(mail);
            }
            catch (Exception ex) { ex.ToString(); }
        }
    }
}
