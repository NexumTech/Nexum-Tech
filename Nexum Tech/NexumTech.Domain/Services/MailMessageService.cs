using Microsoft.Extensions.Localization;
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
        private readonly IStringLocalizer<MailMessageService> _localizer;

        public MailMessageService(ITokenService tokenService, IOptions<AppSettingsAPI> appSettingsAPI, IStringLocalizer<MailMessageService> localizer)
        {
            _tokenService = tokenService;
            _appSettingsAPI = appSettingsAPI.Value;
            _localizer = localizer;
        }

        public string GetMailMessage(MailTypeEnum mailType, UserViewModel user)
        {
            switch (mailType)
            {
                case MailTypeEnum.Register:
                    return RegisterMessage(user);

                case MailTypeEnum.PasswordReset:
                    return PasswordResetMessage(user);

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
            message.Append("<td style=\"width:128px;\">");
            message.Append("<img height=\"auto\" src=\"https://github.com/NexumTech/Nexum-Tech/assets/78672277/c0a8faf4-f41f-4fcd-8702-6825f519d8f5\" style=\"border:0;display:block;outline:none;text-decoration:none;width:100%;\" width=\"128\" />");
            message.Append("</td>");
            message.Append("</tr>");
            message.Append("</tbody>");
            message.Append("</table>");
            message.Append("</td>");
            message.Append("</tr>");
            message.Append("<tr>");
            message.Append("<td align=\"center\" style=\"font-size:0px;padding:10px 25px;padding-bottom:20px;word-break:break-word;padding-top:28px;\">");
            message.Append("<div style=\"font-family:'Helvetica Neue',Arial,sans-serif;font-size:28px;font-weight:bold;line-height:1;text-align:center;color:#555;\">");
            message.Append("Password recovery");
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
            message.Append("<td align=\"center\" style=\"font-size:0px;padding:10px 25px;padding-top:4px;padding-bottom:40px;word-break:break-word;\">");
            message.Append("<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\" style=\"border-collapse:separate;line-height:100%;\">");
            message.Append("<tr>");
            message.Append("<td align=\"center\" bgcolor=\"#2F67F6\" role=\"presentation\" style=\"border:none;border-radius:3px;color:#ffffff;cursor:pointer;padding:15px 25px;\" valign=\"middle\">");
            message.Append($"<a href=\"{passwordResetURL}\" style=\"cursor:pointer;background:#2F67F6;color:#ffffff;font-family:'Helvetica Neue',Arial,sans-serif;font-size:12px;font-weight:normal;line-height:120%;Margin:0;text-decoration:none;text-transform:none;display:inline-block;border-radius:5px;\">");
            message.Append("Reset Password");
            message.Append("</a>");
            message.Append("</td>");
            message.Append("</tr>");
            message.Append("</table>");
            message.Append("</td>");
            message.Append("</tr>");
            message.Append("<tr>");
            message.Append("<td align=\"center\" style=\"font-size:0px;padding:10px 25px;padding-bottom:40px;word-break:break-word;\">");
            message.Append("<div style=\"font-family:'Helvetica Neue',Arial,sans-serif;font-size:16px;line-height:20px;text-align:center;color:#7F8FA4;\">");
            message.Append("If you did not make this request, just ignore this email.");
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
            message.Append("<td style=\"width:128px;\">");
            message.Append("<img height=\"auto\" src=\"https://github.com/NexumTech/Nexum-Tech/assets/78672277/c0a8faf4-f41f-4fcd-8702-6825f519d8f5\" style=\"border:0;display:block;outline:none;text-decoration:none;width:100%;\" width=\"128\" />");
            message.Append("</td>");
            message.Append("</tr>");
            message.Append("</tbody>");
            message.Append("</table>");
            message.Append("</td>");
            message.Append("</tr>");
            message.Append("<tr>");
            message.Append("<td align=\"center\" style=\"font-size:0px;padding:10px 25px;padding-bottom:20px;word-break:break-word;padding-top:28px;\">");
            message.Append("<div style=\"font-family:'Helvetica Neue',Arial,sans-serif;font-size:28px;font-weight:bold;line-height:1;text-align:center;color:#555;\">");
            message.Append("Welcome to Nexum");
            message.Append("</div>");
            message.Append("</td>");
            message.Append("</tr>");
            message.Append("<tr>");
            message.Append("<td align=\"center\" style=\"font-size:0px;padding:10px 25px;padding-bottom:20px;word-break:break-word;\">");
            message.Append("<div style=\"font-family:'Helvetica Neue',Arial,sans-serif;font-size:18px;line-height:1;text-align:center;color:#555;\">");
            message.Append("It seems that you’ve forgotten your password.");
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
    }
}
