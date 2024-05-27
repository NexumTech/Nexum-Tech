using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexumTech.Domain.Interfaces;
using NexumTech.Infra.Models;

namespace NexumTech.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeesService _employeesService;
        private readonly ICompanyService _companyService;

        public EmployeesController(IEmployeesService employeesService, ICompanyService companyService)
        {
            _employeesService = employeesService;
            _companyService = companyService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<EmployeesViewModel>>> GetEmployees(EmployeesViewModel employeesViewModel)
        {
            try
            {
                return Ok(await _employeesService.GetEmployees(employeesViewModel.CompanyId));
            } 
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> AddEmployee(EmployeesViewModel employeesViewModel)
        {
            try
            {
                await _employeesService.AddEmployee(employeesViewModel);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<ActionResult> RemoveEmployee(EmployeesViewModel employeesViewModel)
        {
            try
            {
                await _employeesService.RemoveEmployee(employeesViewModel.Id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
