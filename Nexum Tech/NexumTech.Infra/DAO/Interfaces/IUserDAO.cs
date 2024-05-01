using NexumTech.Infra.Models;

namespace NexumTech.Infra.DAO.Interfaces
{
    public interface IUserDAO
    {
        public Task<UserViewModel> GetUserInfo(LoginViewModel loginViewModel);
        public Task<bool> CheckUserExists(string email);
        public Task<bool> CreateUser(SignupViewModel signupViewModel);
    }
}
