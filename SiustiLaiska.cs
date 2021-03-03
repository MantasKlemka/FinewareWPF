using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace FinewareWPF
{
    class SiustiLaiska
    {
        public static MailMessage CreateMessage(string to, string from, string messageBody, string subject)
        {
            //pranesimo kurimas
            MailMessage message = new MailMessage();
            message.To.Add(to);
            message.From = new MailAddress(from);
            message.Body = messageBody;
            message.Subject = subject;
            return message;
        }

        public static void SendMessage(string from, string pass, MailMessage message)
        {
            //pranesimo siuntimas
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.EnableSsl = true;
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new NetworkCredential(from, pass);
            smtp.Send(message);
        }
    }
}
