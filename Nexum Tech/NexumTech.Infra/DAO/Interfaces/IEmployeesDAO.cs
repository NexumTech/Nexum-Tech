using NexumTech.Infra.Models;

namespace NexumTech.Infra.DAO.Interfaces
{
    public interface IEmployeesDAO
    {
        public Task<IEnumerable<EmployeesViewModel>> GetEmployees(int? companyId);
        public Task<bool> RemoveEmployee(int? employeeId);
        public Task<bool> AddEmployee(int? userId, int? companyId);
        public Task<bool> CheckEmployeeAlreadyExists(int? userId, int? companyId);
    }
}
