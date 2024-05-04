using NexumTech.Infra.Models;

namespace NexumTech.Domain.Interfaces
{
    public interface IUserService
    {
        public Task<UserViewModel> GetUserInfo(LoginViewModel loginViewModel);
        public Task<UserViewModel> GetUserInfo(string email);
        public Task<bool> CreateUser(SignupViewModel signupViewModel);
    }
}
