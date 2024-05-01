using NexumTech.Infra.Models;

namespace NexumTech.Infra.API.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(UserViewModel user);
    }
}
