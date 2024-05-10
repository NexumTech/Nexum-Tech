using Dapper;
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

        public async Task<UserViewModel> GetUserInfo(string email)
        {
            try
            {
                using (var connection = _baseDatabaseService.GetConnection())
                {
                    connection.Open();

                    string sql = @"SELECT 
                                        Id,
                                        Email,
                                        Username,
                                        Photo,
                                        Role
                                   FROM tb_user 
                                   WHERE Email = @Email";

                    var user = await connection.QueryFirstOrDefaultAsync<UserViewModel>(sql, new
                    {
                        @Email = email,
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
                                        ,[Photo]
                                        ,[Role])
                                    VALUES
                                        (@Username
                                        ,@Email
                                        ,@Password
                                        ,@Photo
                                        ,'User')";

                    await connection.QueryAsync(sql, new
                    {
                        @Username = signupViewModel.Username,
                        @Email = signupViewModel.Email,
                        @Password = signupViewModel.Password,
                        @Photo = signupViewModel.Photo,
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

                    string sql = "SELECT COUNT(*) FROM tb_user WHERE Email = @Email and Password IS NOT NULL";

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

        public async Task<bool> UpdateUser(int id, string username, byte[] photo)
        {
            try
            {
                using (var connection = _baseDatabaseService.GetConnection())
                {
                    connection.Open();

                    string sql = "UPDATE tb_user SET Photo = @Photo, Username = @Username WHERE Id = @Id";

                    await connection.QueryFirstOrDefaultAsync<int>(sql, new
                    {
                        @Id = id,
                        @Username = username,
                        @Photo = photo
                    });

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdatePassword(string password, string email)
        {
            try
            {
                using (var connection = _baseDatabaseService.GetConnection())
                {
                    connection.Open();

                    string sql = "UPDATE tb_user SET Password = @Password WHERE Email = @Email";

                    var userExists = await connection.QueryFirstOrDefaultAsync<int>(sql, new
                    {
                        @Password = password,
                        @Email = email
                    });

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
