using NexumTech.Infra.Models;

namespace NexumTech.Domain.Interfaces
{
    public interface IUserService
    {
        public Task<UserViewModel> GetUserInfo(LoginViewModel loginViewModel);
        public Task<bool> CreateUser(SignupViewModel signupViewModel);
    }
}
