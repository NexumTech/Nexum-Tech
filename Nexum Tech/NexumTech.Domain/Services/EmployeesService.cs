using Microsoft.Extensions.Localization;
using NexumTech.Domain.Interfaces;
using NexumTech.Infra.DAO.Interfaces;
using NexumTech.Infra.Models;

namespace NexumTech.Domain.Services
{
    public class EmployeesService : IEmployeesService
    {
        private readonly IUserService _userService;
        private readonly IEmployeesDAO _employeesDAO;
        private readonly IStringLocalizer<EmployeesService> _localizer;

        public EmployeesService(IEmployeesDAO employeesDAO, IStringLocalizer<EmployeesService> localizer, IUserService userService)
        {
            _employeesDAO = employeesDAO;
            _localizer = localizer;
            _userService = userService;
        }

        public async Task<IEnumerable<EmployeesViewModel>> GetEmployees(int? companyId)
        {
            return await _employeesDAO.GetEmployees(companyId);
        }

        public async Task<bool> AddEmployee(EmployeesViewModel employeesViewModel)
        {
            UserViewModel user = await _userService.GetUserInfo(employeesViewModel.Email);

            if (user == null) throw new Exception(_localizer["UserNotFound"]);

            return await _employeesDAO.AddEmployee(user.Id, employeesViewModel.CompanyId);
        }

        public async Task<bool> RemoveEmployee(int? employeeId)
        {
            return await _employeesDAO.RemoveEmployee(employeeId);
        }
    }
}
