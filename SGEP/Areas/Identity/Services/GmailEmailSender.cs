using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace SGEP.Areas.Identity.Services
{
    public class GmailEmailSender : IEmailSender
    {
        const string EMAIL = "sgep.noreply@gmail.com", PASSWORD = "ggdTWADsma8E1Ud";
        
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);

            smtp.Credentials = new NetworkCredential(EMAIL, PASSWORD);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;

            MailMessage message = new MailMessage();
            message.From = new MailAddress(EMAIL, "SGEP");
            message.To.Add(email);
            message.IsBodyHtml = true;
            message.Subject = subject;
            message.Body = htmlMessage;
            await smtp.SendMailAsync(message);
        }
    }
}