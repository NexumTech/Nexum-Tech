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

            message.Append($"<a href='{passwordResetURL}'>Redefinir senha</a>");

            return message.ToString();
        }

        private string RegisterMessage(UserViewModel user)
        {
            return "";
        }

        private string CompanyMessage()
        {
            return "";
        }
    }
}
