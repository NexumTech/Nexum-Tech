using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexumTech.Domain.Interfaces;
using NexumTech.Infra.Models;

namespace NexumTech.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CompanyController : Controller
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetCompany(CompanyViewModel companyViewModel)
        {
            try
            {
                return Ok(await _companyService.GetCompany(companyViewModel.OwnerId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CompanyViewModel>>> GetCompanies(UserViewModel userViewModel)
        {
            try
            {
                return Ok(await _companyService.GetCompanies(userViewModel.Id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> CheckCompanyOwner(EmployeesViewModel employeesViewModel)
        {
            try
            {
                bool isOwner = await _companyService.CheckCompanyOwner(employeesViewModel.CompanyId, employeesViewModel.Id);

                if (!isOwner) return BadRequest();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateCompany(CompanyViewModel companyViewModel)
        {
            try
            {
                await _companyService.CreateCompany(companyViewModel);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult> UpdateCompany(CompanyViewModel companyViewModel)
        {
            try
            {
                await _companyService.UpdateCompany(companyViewModel);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<ActionResult> DeleteCompany(CompanyViewModel companyViewModel)
        {
            try
            {
                await _companyService.DeleteCompany(companyViewModel.OwnerId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
