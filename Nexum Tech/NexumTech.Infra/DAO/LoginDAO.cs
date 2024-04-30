using Nexum_Tech.Infra.DAO.Interfaces;
using NexumTech.Infra.Models;
using System.Data.SqlClient;

namespace Nexum_Tech.Infra.DAO
{
    public class LoginDAO : ILoginDAO
    {
        private readonly BaseDatabaseService _baseDatabaseService;

        public LoginDAO(BaseDatabaseService baseDatabaseService)
        {
            _baseDatabaseService = baseDatabaseService;
        }

        public int Authentication(LoginViewModel loginViewModel)
        {
            try
            {
                using (var connection = (SqlConnection)_baseDatabaseService.GetConnection())
                {
                    connection.Open();

                    string sql = "SELECT COUNT(*) FROM tb_user WHERE Email = @Email AND Password = @Password";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Email", loginViewModel.Email);
                        command.Parameters.AddWithValue("@Password", loginViewModel.Password);

                        int count = (int)command.ExecuteScalar();

                        return (int)command.ExecuteScalar() > 0 ? 1 : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao autenticar o usuário: " + ex.Message);
            }
        }
    }
}
