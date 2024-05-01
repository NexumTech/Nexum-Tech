using Dapper;
using NexumTech.Infra.DAO;
using NexumTech.Infra.DAO.Interfaces;
using NexumTech.Infra.Models;

namespace NexumTech.Infra.DAO
{
    public class UserDAO : IUserDAO
    {
        private readonly BaseDatabaseService _baseDatabaseService;

        public UserDAO(BaseDatabaseService baseDatabaseService)
        {
            _baseDatabaseService = baseDatabaseService;
        }

        public async Task<UserViewModel> GetUserInfo(LoginViewModel loginViewModel)
        {
            try
            {
                using (var connection = _baseDatabaseService.GetConnection())
                {
                    connection.Open();

                    string sql = "SELECT * FROM tb_user WHERE Email = @Email AND Password = @Password";

                    var user = await connection.QueryFirstOrDefaultAsync<UserViewModel>(sql, new {
                        @Email = loginViewModel.Email,
                        @Password = loginViewModel.Password,
                    });

                    return user;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> CreateUser(SignupViewModel signupViewModel)
        {
            try
            {
                using (var connection = _baseDatabaseService.GetConnection())
                {
                    connection.Open();

                    string sql = @"INSERT INTO tb_user
                                        ([Username]
                                        ,[Email]
                                        ,[Password]
                                        ,[Role])
                                    VALUES
                                        (@Username
                                        ,@Email
                                        ,@Password
                                        ,'User')";

                    await connection.QueryAsync(sql, new
                    {
                        @Username = signupViewModel.Username,
                        @Email = signupViewModel.Email,
                        @Password = signupViewModel.Password,
                    });

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> CheckUserExists(string email)
        {
            try
            {
                using (var connection = _baseDatabaseService.GetConnection())
                {
                    connection.Open();

                    string sql = "SELECT COUNT(*) FROM tb_user WHERE Email = @Email";

                    var userExists = await connection.QueryFirstOrDefaultAsync<int>(sql, new
                    {
                        @Email = email,
                    });

                    return (userExists == 1);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
