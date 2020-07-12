using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace SGEP.Areas.Identity.Services
{
    public class EmailSender : IEmailSender
    {
        const string EMAIL = "sgep.noreply@gmail.com", PASSWORD = "ggdTWADsma8E1Ud";
        public static EmailServiceConfiguration Configuration { get; set; }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            SmtpClient smtp = new SmtpClient(Configuration.SmtpAddress, Configuration.SmtpPort);

            smtp.Credentials = new NetworkCredential(Configuration.Email, Configuration.Password);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;

            MailMessage message = new MailMessage();
            message.From = new MailAddress(Configuration.Email, "SGEP");
            message.To.Add(email);
            message.IsBodyHtml = true;
            message.Subject = subject;
            message.Body = htmlMessage;
            await smtp.SendMailAsync(message);
        }
    }

    public class EmailServiceConfiguration
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string SmtpAddress { get; set; }
        public int SmtpPort { get; set; }
    }
}