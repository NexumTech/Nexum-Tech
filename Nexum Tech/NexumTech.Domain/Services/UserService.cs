
using NexumTech.Domain.Interfaces;
using NexumTech.Infra.DAO.Interfaces;
using NexumTech.Infra.Models;

namespace NexumTech.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserDAO _userDAO;
        public UserService(IUserDAO userDAO) 
        { 
            _userDAO = userDAO;
        }

        public async Task<UserViewModel> GetUserInfo(LoginViewModel loginViewModel)
        {
            var user = await _userDAO.GetUserInfo(loginViewModel);

            if (user == null) throw new Exception("Invalid credentials!");

            return user;
        }

        public async Task<bool> CreateUser(SignupViewModel signupViewModel)
        {

            bool userExists = await _userDAO.CheckUserExists(signupViewModel.Email);

            if (userExists) throw new Exception("User already exists!");

            return await _userDAO.CreateUser(signupViewModel);
        }
    }
}
