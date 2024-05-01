using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace NexumTech.Infra.DAO
{
    public class BaseDatabaseService
    {
        private readonly string? _connectionString;

        public BaseDatabaseService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
