namespace NexumTech.Infra.API.Interfaces
{
    public interface IMailService
    {
        public Task<bool> SendMail(string to, string subject, string message);
    }
}
