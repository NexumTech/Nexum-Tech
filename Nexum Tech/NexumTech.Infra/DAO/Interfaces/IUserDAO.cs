using NexumTech.Infra.Models;

namespace NexumTech.Infra.DAO.Interfaces
{
    public interface IUserDAO
    {
        public Task<UserViewModel> GetUserInfo(LoginViewModel loginViewModel);
        public Task<UserViewModel> GetUserInfo(string email);
        public Task<bool> CheckUserExists(string email);
        public Task<bool> CreateUser(SignupViewModel signupViewModel);
        public Task<bool> UpdateUser(int id, string email, byte[] photo);
        public Task<bool> UpdatePassword(string password, string email);
    }
}
