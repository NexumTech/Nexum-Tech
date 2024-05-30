using NexumTech.Infra.Models;

namespace NexumTech.Domain.Interfaces
{
    public interface IEmployeesService
    {
        public Task<IEnumerable<EmployeesViewModel>> GetEmployees(int? companyId);
        public Task<bool> RemoveEmployee(int? employeeId);
        public Task<bool> AddEmployee(EmployeesViewModel employeesViewModel);
    }
}
