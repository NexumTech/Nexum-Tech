using Microsoft.Extensions.Options;
using NexumTech.Infra.API.Interfaces;
using System.Net;
using System.Net.Mail;

namespace NexumTech.Infra.API
{
    public class MailService: IMailService
    {
        private readonly AppSettingsAPI _appSettingsAPI;

        public MailService(IOptions<AppSettingsAPI> appSettingsAPI) 
        { 
        _appSettingsAPI = appSettingsAPI.Value;
        }

        public async Task<bool> SendMail(string to, string subject, string message)
        {
            try
            {
                string host = _appSettingsAPI.SMTP.Host;
                string name = _appSettingsAPI.SMTP.Name;
                string username = _appSettingsAPI.SMTP.Username;
                string password = _appSettingsAPI.SMTP.Password;
                int port = Convert.ToInt16(_appSettingsAPI.SMTP.Port);

                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(username, name),
                };

                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                using(SmtpClient smtp = new SmtpClient(host, port))
                {
                    smtp.Credentials = new NetworkCredential(username, password);
                    smtp.EnableSsl = true;

                    smtp.Send(mail);
                }

                return true;
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
