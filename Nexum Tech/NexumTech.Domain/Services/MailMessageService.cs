using Microsoft.Extensions.Options;
using NexumTech.Domain.Enums;
using NexumTech.Domain.Interfaces;
using NexumTech.Infra.API;
using NexumTech.Infra.API.Interfaces;
using NexumTech.Infra.Models;
using System.Text;

namespace NexumTech.Domain.Services
{
    public class MailMessageService : IMailMessageService
    {
        private readonly ITokenService _tokenService;
        private readonly AppSettingsAPI _appSettingsAPI;

        public MailMessageService(ITokenService tokenService, IOptions<AppSettingsAPI> appSettingsAPI)
        {
            _tokenService = tokenService;
            _appSettingsAPI = appSettingsAPI.Value;
        }

        public string GetMailMessage(MailTypeEnum mailType, UserViewModel user)
        {
            switch (mailType)
            {
                case MailTypeEnum.Register:
                    return RegisterMessage(user);

                case MailTypeEnum.PasswordReset:
                    return PasswordResetMessage(user);

                case MailTypeEnum.Company:
                    return CompanyMessage();

                default:
                    return String.Empty;
            }
        }

        private string PasswordResetMessage(UserViewModel user)
        {
            string passwordResetToken = _tokenService.GenerateToken(user, true);

            string passwordResetURL = $"{_appSettingsAPI.PasswordResetURL}?token={passwordResetToken}";

            StringBuilder message = new StringBuilder();

            message.Append("<html><head>");
            message.Append("<title></title>");
            message.Append("<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">");
            message.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">");
            message.Append("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">");
            message.Append("<style type=\"text/css\">");
            message.Append("#outlook a { padding: 0; }");
            message.Append(".ReadMsgBody { width: 100%; }");
            message.Append(".ExternalClass { width: 100%; }");
            message.Append(".ExternalClass * { line-height: 100%; }");
            message.Append("body { margin: 0; padding: 0; -webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%; }");
            message.Append("table, td { border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; }");
            message.Append("img { border: 0; height: auto; line-height: 100%; outline: none; text-decoration: none; -ms-interpolation-mode: bicubic; }");
            message.Append("p { display: block; margin: 13px 0; }");
            message.Append("</style>");
            message.Append("<!--[if !mso]><!-->");
            message.Append("<style type=\"text/css\">");
            message.Append("@media only screen and (max-width:480px) {");
            message.Append("@-ms-viewport { width: 320px; }");
            message.Append("@viewport { width: 320px; }");
            message.Append("}");
            message.Append("</style>");
            message.Append("<style type=\"text/css\">");
            message.Append("@media only screen and (min-width:480px) {");
            message.Append(".mj-column-per-100 { width: 100% !important; }");
            message.Append("}");
            message.Append("</style>");
            message.Append("<style type=\"text/css\"></style>");
            message.Append("</head>");
            message.Append("<body style=\"background-color:#f9f9f9;\">");
            message.Append("<div style=\"background-color:#f9f9f9;\">");
            message.Append("<div style=\"background:#f9f9f9;background-color:#f9f9f9;Margin:0px auto;max-width:600px;\">");
            message.Append("<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"background:#f9f9f9;background-color:#f9f9f9;width:100%;\">");
            message.Append("<tbody>");
            message.Append("<tr>");
            message.Append("<td style=\"border-bottom:#333957 solid 5px;direction:ltr;font-size:0px;padding:20px 0;text-align:center;vertical-align:top;\"></td>");
            message.Append("</tr>");
            message.Append("</tbody>");
            message.Append("</table>");
            message.Append("</div>");
            message.Append("<div style=\"background:#fff;background-color:#fff;Margin:0px auto;max-width:600px;\">");
            message.Append("<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"background:#fff;background-color:#fff;width:100%;\">");
            message.Append("<tbody>");
            message.Append("<tr>");
            message.Append("<td style=\"border:#dddddd solid 1px;border-top:0px;direction:ltr;font-size:0px;padding:20px 0;text-align:center;vertical-align:top;\">");
            message.Append("<div class=\"mj-column-per-100 outlook-group-fix\" style=\"font-size:13px;text-align:left;direction:ltr;display:inline-block;vertical-align:bottom;width:100%;\">");
            message.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"vertical-align:bottom;\" width=\"100%\">");
            message.Append("<tr>");
            message.Append("<td align=\"center\" style=\"font-size:0px;padding:10px 25px;word-break:break-word;\">");
            message.Append("<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"border-collapse:collapse;border-spacing:0px;\">");
            message.Append("<tbody>");
            message.Append("<tr>");
            message.Append("<td style=\"width:64px;\">");
            message.Append("<img height=\"auto\" src=\"https://i.imgur.com/KO1vcE9.png\" style=\"border:0;display:block;outline:none;text-decoration:none;width:100%;\" width=\"64\" />");
            message.Append("</td>");
            message.Append("</tr>");
            message.Append("</tbody>");
            message.Append("</table>");
            message.Append("</td>");
            message.Append("</tr>");
            message.Append("<tr>");
            message.Append("<td align=\"center\" style=\"font-size:0px;padding:10px 25px;padding-bottom:40px;word-break:break-word;\">");
            message.Append("<div style=\"font-family:'Helvetica Neue',Arial,sans-serif;font-size:38px;font-weight:bold;line-height:1;text-align:center;color:#555;\">");
            message.Append("Oops!");
            message.Append("</div>");
            message.Append("</td>");
            message.Append("</tr>");
            message.Append("<tr>");
            message.Append("<td align=\"center\" style=\"font-size:0px;padding:10px 25px;padding-bottom:40px;word-break:break-word;\">");
            message.Append("<div style=\"font-family:'Helvetica Neue',Arial,sans-serif;font-size:18px;line-height:1;text-align:center;color:#555;\">");
            message.Append("It seems that you’ve forgotten your password.");
            message.Append("</div>");
            message.Append("</td>");
            message.Append("</tr>");
            message.Append("<tr>");
            message.Append("<td align=\"center\" style=\"font-size:0px;padding:10px 25px;word-break:break-word;\">");
            message.Append("<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"border-collapse:collapse;border-spacing:0px;\">");
            message.Append("<tbody>");
            message.Append("<tr>");
            message.Append("<td style=\"width:128px;\">");
            message.Append("<img height=\"auto\" src=\"https://i.imgur.com/247tYSw.png\" style=\"border:0;display:block;outline:none;text-decoration:none;width:100%;\" width=\"128\" />");
            message.Append("</td>");
            message.Append("</tr>");
            message.Append("</tbody>");
            message.Append("</table>");
            message.Append("</td>");
            message.Append("</tr>");
            message.Append("<tr>");
            message.Append("<td align=\"center\" style=\"font-size:0px;padding:10px 25px;padding-top:30px;padding-bottom:50px;word-break:break-word;\">");
            message.Append("<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"border-collapse:separate;line-height:100%;\">");
            message.Append("<tr>");
            message.Append("<td align=\"center\" bgcolor=\"#2F67F6\" role=\"presentation\" style=\"border:none;border-radius:3px;color:#ffffff;cursor:auto;padding:15px 25px;\" valign=\"middle\">");
            message.Append("<p style=\"background:#2F67F6;color:#ffffff;font-family:'Helvetica Neue',Arial,sans-serif;font-size:15px;font-weight:normal;line-height:120%;Margin:0;text-decoration:none;text-transform:none;\">");
            message.Append("Reset Password");
            message.Append("</p>");
            message.Append("</td>");
            message.Append("</tr>");
            message.Append("</table>");
            message.Append("</td>");
            message.Append("</tr>");
            message.Append("<tr>");
            message.Append("<td align=\"center\" style=\"font-size:0px;padding:10px 25px;padding-bottom:40px;word-break:break-word;\">");
            message.Append("<div style=\"font-family:'Helvetica Neue',Arial,sans-serif;font-size:16px;line-height:20px;text-align:center;color:#7F8FA4;\">");
            message.Append("If you did not make this request, just ignore this email. Otherwise please click the button above to reset your password.");
            message.Append("</div>");
            message.Append("</td>");
            message.Append("</tr>");
            message.Append("</table>");
            message.Append("</div>");
            message.Append("</td>");
            message.Append("</tr>");
            message.Append("</tbody>");
            message.Append("</table>");
            message.Append("</div>");
            message.Append("<div style=\"Margin:0px auto;max-width:600px;\">");
            message.Append("<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"width:100%;\">");
            message.Append("<tbody>");
            message.Append("<tr>");
            message.Append("<td style=\"direction:ltr;font-size:0px;padding:20px 0;text-align:center;vertical-align:top;\">");
            message.Append("<div class=\"mj-column-per-100 outlook-group-fix\" style=\"font-size:13px;text-align:left;direction:ltr;display:inline-block;vertical-align:bottom;width:100%;\">");
            message.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" width=\"100%\">");
            message.Append("<tbody>");
            message.Append("<tr>");
            message.Append("<td style=\"vertical-align:bottom;padding:0;\">");
            message.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" width=\"100%\">");
            message.Append("<tr>");
            message.Append("<td align=\"center\" style=\"font-size:0px;padding:0;word-break:break-word;\">");
            message.Append("<div style=\"font-family:'Helvetica Neue',Arial,sans-serif;font-size:12px;font-weight:300;line-height:1;text-align:center;color:#575757;\">");
            message.Append("Some Firm Ltd, 35 Avenue. City 10115, USA");
            message.Append("</div>");
            message.Append("</td>");
            message.Append("</tr>");
            message.Append("<tr>");
            message.Append("<td align=\"center\" style=\"font-size:0px;padding:10px;word-break:break-word;\">");
            message.Append("<div style=\"font-family:'Helvetica Neue',Arial,sans-serif;font-size:12px;font-weight:300;line-height:1;text-align:center;color:#575757;\">");
            message.Append("<a href=\"\" style=\"color:#575757\">Unsubscribe</a> from our emails");
            message.Append("</div>");
            message.Append("</td>");
            message.Append("</tr>");
            message.Append("</table>");
            message.Append("</td>");
            message.Append("</tr>");
            message.Append("</tbody>");
            message.Append("</table>");
            message.Append("</div>");
            message.Append("</td>");
            message.Append("</tr>");
            message.Append("</tbody>");
            message.Append("</table>");
            message.Append("</div>");
            message.Append("</body></html>");


            return message.ToString();
        }

        private string RegisterMessage(UserViewModel user)
        {
            StringBuilder message = new StringBuilder();

            message.Append("<html><head>");
            message.Append("<title></title>");
            message.Append("<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">");
            message.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\">");
            message.Append("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">");
            message.Append("<style type=\"text/css\">");
            message.Append("#outlook a { padding: 0; }");
            message.Append(".ReadMsgBody { width: 100%; }");
            message.Append(".ExternalClass { width: 100%; }");
            message.Append(".ExternalClass * { line-height: 100%; }");
            message.Append("body { margin: 0; padding: 0; -webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%; }");
            message.Append("table, td { border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; }");
            message.Append("img { border: 0; height: auto; line-height: 100%; outline: none; text-decoration: none; -ms-interpolation-mode: bicubic; }");
            message.Append("p { display: block; margin: 13px 0; }");
            message.Append("</style>");
            message.Append("<style type=\"text/css\">");
            message.Append("@media only screen and (max-width:480px) {");
            message.Append("@-ms-viewport { width: 320px; }");
            message.Append("@viewport { width: 320px; }");
            message.Append("}");
            message.Append("</style>");
            message.Append("<style type=\"text/css\">");
            message.Append("@media only screen and (min-width:480px) {");
            message.Append(".mj-column-per-100 { width: 100% !important; }");
            message.Append("}");
            message.Append("</style>");
            message.Append("<style type=\"text/css\"></style>");
            message.Append("</head>");
            message.Append("<body style=\"background-color:#f9f9f9;\">");
            message.Append("<div style=\"background-color:#f9f9f9;\">");
            message.Append("<div style=\"background:#f9f9f9;background-color:#f9f9f9;Margin:0px auto;max-width:600px;\">");
            message.Append("<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"background:#f9f9f9;background-color:#f9f9f9;width:100%;\">");
            message.Append("<tbody>");
            message.Append("<tr>");
            message.Append("<td style=\"border-bottom:#333957 solid 5px;direction:ltr;font-size:0px;padding:20px 0;text-align:center;vertical-align:top;\"></td>");
            message.Append("</tr>");
            message.Append("</tbody>");
            message.Append("</table>");
            message.Append("</div>");
            message.Append("<div style=\"background:#fff;background-color:#fff;Margin:0px auto;max-width:600px;\">");
            message.Append("<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"background:#fff;background-color:#fff;width:100%;\">");
            message.Append("<tbody>");
            message.Append("<tr>");
            message.Append("<td style=\"border:#dddddd solid 1px;border-top:0px;direction:ltr;font-size:0px;padding:20px 0;text-align:center;vertical-align:top;\">");
            message.Append("<div class=\"mj-column-per-100 outlook-group-fix\" style=\"font-size:13px;text-align:left;direction:ltr;display:inline-block;vertical-align:bottom;width:100%;\">");
            message.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"vertical-align:bottom;\" width=\"100%\">");
            message.Append("<tr>");
            message.Append("<td align=\"center\" style=\"font-size:0px;padding:10px 25px;word-break:break-word;\">");
            message.Append("<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"border-collapse:collapse;border-spacing:0px;\">");
            message.Append("<tbody>");
            message.Append("<tr>");
            message.Append("<td style=\"width:64px;\">");
            message.Append("<img height=\"auto\" src=\"https://i.imgur.com/KO1vcE9.png\" style=\"border:0;display:block;outline:none;text-decoration:none;width:100%;\" width=\"64\" />");
            message.Append("</td>");
            message.Append("</tr>");
            message.Append("</tbody>");
            message.Append("</table>");
            message.Append("</td>");
            message.Append("</tr>");
            message.Append("<tr>");
            message.Append("<td align=\"center\" style=\"font-size:0px;padding:10px 25px;padding-bottom:40px;word-break:break-word;\">");
            message.Append("<div style=\"font-family:'Helvetica Neue',Arial,sans-serif;font-size:28px;font-weight:bold;line-height:1;text-align:center;color:#555;\">");
            message.Append("Welcome to {{Product}}");
            message.Append("</div>");
            message.Append("</td>");
            message.Append("</tr>");
            message.Append("<tr>");
            message.Append("<td align=\"left\" style=\"font-size:0px;padding:10px 25px;word-break:break-word;\">");
            message.Append("<div style=\"font-family:'Helvetica Neue',Arial,sans-serif;font-size:16px;line-height:22px;text-align:left;color:#555;\">");
            message.Append("Hello {{ name }}!<br></br>");
            message.Append("Thank you for signing up for {{ product }}. We're really happy to have you! Click the link below to login to your account:");
            message.Append("</div>");
            message.Append("</td>");
            message.Append("</tr>");
            message.Append("<tr>");
            message.Append("<td align=\"center\" style=\"font-size:0px;padding:10px 25px;padding-top:30px;padding-bottom:50px;word-break:break-word;\">");
            message.Append("<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"border-collapse:separate;line-height:100%;\">");
            message.Append("<tr>");
            message.Append("<td align=\"center\" bgcolor=\"#2F67F6\" role=\"presentation\" style=\"border:none;border-radius:3px;color:#ffffff;cursor:auto;padding:15px 25px;\" valign=\"middle\">");
            message.Append("<p style=\"background:#2F67F6;color:#ffffff;font-family:'Helvetica Neue',Arial,sans-serif;font-size:15px;font-weight:normal;line-height:120%;Margin:0;text-decoration:none;text-transform:none;\">");
            message.Append("Login to Your Account");
            message.Append("</p>");
            message.Append("</td>");
            message.Append("</tr>");
            message.Append("</table>");
            message.Append("</td>");
            message.Append("</tr>");
            message.Append("<tr>");
            message.Append("<td align=\"left\" style=\"font-size:0px;padding:10px 25px;word-break:break-word;\">");
            message.Append("<div style=\"font-family:'Helvetica Neue',Arial,sans-serif;font-size:14px;line-height:20px;text-align:left;color:#525252;\">");
            message.Append("Best regards,<br><br> Csaba Kissi<br>Elerion ltd., CEO and Founder<br>");
            message.Append("<a href=\"https://www.htmlemailtemplates.net\" style=\"color:#2F67F6\">htmlemailtemplates.net</a>");
            message.Append("</div>");
            message.Append("</td>");
            message.Append("</tr>");
            message.Append("</table>");
            message.Append("</div>");
            message.Append("</td>");
            message.Append("</tr>");
            message.Append("</tbody>");
            message.Append("</table>");
            message.Append("</div>");
            message.Append("<div style=\"Margin:0px auto;max-width:600px;\">");
            message.Append("<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"width:100%;\">");
            message.Append("<tbody>");
            message.Append("<tr>");
            message.Append("<td style=\"direction:ltr;font-size:0px;padding:20px 0;text-align:center;vertical-align:top;\">");
            message.Append("<div class=\"mj-column-per-100 outlook-group-fix\" style=\"font-size:13px;text-align:left;direction:ltr;display:inline-block;vertical-align:bottom;width:100%;\">");
            message.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" width=\"100%\">");
            message.Append("<tbody>");
            message.Append("<tr>");
            message.Append("<td style=\"vertical-align:bottom;padding:0;\">");
            message.Append("<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" width=\"100%\">");
            message.Append("<tr>");
            message.Append("<td align=\"center\" style=\"font-size:0px;padding:0;word-break:break-word;\">");
            message.Append("<div style=\"font-family:'Helvetica Neue',Arial,sans-serif;font-size:12px;font-weight:300;line-height:1;text-align:center;color:#575757;\">");
            message.Append("Some Firm Ltd, 35 Avenue. City 10115, USA");
            message.Append("</div>");
            message.Append("</td>");
            message.Append("</tr>");
            message.Append("<tr>");
            message.Append("<td align=\"center\" style=\"font-size:0px;padding:10px;word-break:break-word;\">");
            message.Append("<div style=\"font-family:'Helvetica Neue',Arial,sans-serif;font-size:12px;font-weight:300;line-height:1;text-align:center;color:#575757;\">");
            message.Append("<a href=\"\" style=\"color:#575757\">Unsubscribe</a> from our emails");
            message.Append("</div>");
            message.Append("</td>");
            message.Append("</tr>");
            message.Append("</table>");
            message.Append("</td>");
            message.Append("</tr>");
            message.Append("</tbody>");
            message.Append("</table>");
            message.Append("</div>");
            message.Append("</td>");
            message.Append("</tr>");
            message.Append("</tbody>");
            message.Append("</table>");
            message.Append("</div>");
            message.Append("</body></html>");



            return message.ToString();
        }

        private string CompanyMessage()
        {
            return "";
        }
    }
}
