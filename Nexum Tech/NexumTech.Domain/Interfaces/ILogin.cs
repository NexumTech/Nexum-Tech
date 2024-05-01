using NexumTech.Infra.Models;

namespace Nexum_Tech.Domain.Interfaces
{
    public interface ILogin
    {
        public int Authentication(LoginViewModel loginViewModel);
    }
}
