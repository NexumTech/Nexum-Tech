using NexumTech.Domain.Enums;
using NexumTech.Infra.Models;

namespace NexumTech.Domain.Interfaces
{
    public interface IMailMessageService
    {
        public string GetMailMessage(MailTypeEnum mailType, UserViewModel user);
    }
}
