using Dapper;
using NexumTech.Infra.DAO.Interfaces;
using NexumTech.Infra.Models;

namespace NexumTech.Infra.DAO
{
    public class EmployeesDAO: IEmployeesDAO
    {
        private readonly BaseDatabaseService _baseDatabaseService;

        public EmployeesDAO(BaseDatabaseService baseDatabaseService)
        {
            _baseDatabaseService = baseDatabaseService;
        }

        public async Task<bool> CheckEmployeeAlreadyExists(int? userId, int? companyId)
        {
            try
            {
                using (var connection = _baseDatabaseService.GetConnection())
                {
                    connection.Open();

                    string sql = "SELECT COUNT(*) FROM tb_employee WHERE CompanyId = @CompanyId and UserId = @UserId";

                    var employee = await connection.QueryFirstOrDefaultAsync<int>(sql, new
                    {
                        @UserId = userId,
                        @CompanyId = companyId,
                    });

                    return (employee == 1);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<EmployeesViewModel>> GetEmployees(int? companyId)
        {
            try
            {
                using (var connection = _baseDatabaseService.GetConnection())
                {
                    connection.Open();

                    string sql = "SELECT emp.Id, usr.Username, usr.Email FROM tb_employee emp INNER JOIN tb_user usr ON emp.UserId = usr.Id WHERE emp.CompanyId = @CompanyId";

                    var employees = await connection.QueryAsync<EmployeesViewModel>(sql, new
                    {
                        @CompanyId = companyId,
                    });

                    return employees;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> AddEmployee(int? userId, int? companyId)
        {
            try
            {
                using (var connection = _baseDatabaseService.GetConnection())
                {
                    connection.Open();

                    string sql = @"INSERT INTO [dbo].[tb_employee]
                                    ([UserId]
                                    ,[CompanyId])
                                 VALUES
                                    (@UserId
                                    ,@CompanyId)";

                    await connection.QueryAsync(sql, new
                    {
                        @UserId = userId,
                        @CompanyId = companyId,
                    });

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> RemoveEmployee(int? employeeId)
        {
            try
            {
                using (var connection = _baseDatabaseService.GetConnection())
                {
                    connection.Open();

                    string sql = "DELETE FROM tb_employee WHERE Id = @Id";

                    var employees = await connection.QueryAsync<int>(sql, new
                    {
                        @Id = employeeId,
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
