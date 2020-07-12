using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace SGEP.Areas.Identity.Services
{
    ///<summary>
    ///Serviço para enviar e-mails.
    ///</summary>
    public class EmailSender : IEmailSender
    {
        public static EmailServiceConfiguration Configuration { get; set; }
        ///<summary>
        ///Envia um e-mail para "email" com o assunto "subject" e conteúdo "htmlMessage".
        ///<param name="email">E-mail do destinatário</param>
        ///<param name="subject">Assunto do e-mail</param>
        ///<param name="htmlMessage">Conteúdo em html do e-mail</param>
        ///</summary>
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
    ///<summary>
    ///Representa os segredos relacionados ao serviço de e-mail encontrados no arquivo appsettings.json.
    ///Possui apenas uma instância estática em EmailSender. 
    ///</summary>
    public class EmailServiceConfiguration
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string SmtpAddress { get; set; }
        public int SmtpPort { get; set; }
    }
}