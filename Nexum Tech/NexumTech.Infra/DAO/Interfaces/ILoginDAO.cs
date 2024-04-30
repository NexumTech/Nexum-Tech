using NexumTech.Infra.Models;

namespace Nexum_Tech.Infra.DAO.Interfaces
{
    public interface ILoginDAO
    {
        public int Authentication(LoginViewModel loginViewModel);
    }
}
