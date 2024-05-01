using Nexum_Tech.Domain.Interfaces;
using Nexum_Tech.Infra.DAO.Interfaces;
using NexumTech.Infra.Models;

namespace NexumTech.Domain.Services
{
    public class LoginService : ILogin
    {
        private readonly ILoginDAO _loginDAO;

        public LoginService(ILoginDAO loginDAO)
        {
            _loginDAO = loginDAO;
        }

        public int Authentication(LoginViewModel loginViewModel)
        {
            return _loginDAO.Authentication(loginViewModel);
        }
    }
}
