using Microsoft.Extensions.Localization;
using NexumTech.Domain.Interfaces;
using NexumTech.Infra.DAO.Interfaces;
using NexumTech.Infra.Models;
using System.Text.RegularExpressions;

namespace NexumTech.Domain.Services
{
    public class CompanyService: ICompanyService
    {
        private readonly ICompanyDAO _companyDAO;
        private readonly IStringLocalizer<CompanyService> _localizer;

        public CompanyService(ICompanyDAO companyDAO, IStringLocalizer<CompanyService> localizer) 
        { 
            _companyDAO = companyDAO;
            _localizer = localizer;
        }

        public async Task<bool> CreateCompany(CompanyViewModel companyViewModel)
        {
            CompanyViewModel company = await _companyDAO.GetCompany(companyViewModel.Name);

            if (company != null) throw new Exception(_localizer["CompanyExists"]);

            string base64 = String.Empty;
            byte[] logo = new byte[0];

            Regex regex = new Regex(@"^data:image/(jpeg|png);base64,", RegexOptions.IgnoreCase);
            base64 = regex.Replace(companyViewModel.Base64Logo, "");
            logo = Convert.FromBase64String(base64);

            companyViewModel.Logo = logo;

            return await _companyDAO.CreateCompany(companyViewModel);
        }

        public async Task<CompanyViewModel> GetCompany(int ownerId)
        {
            CompanyViewModel company = await _companyDAO.GetCompany(ownerId);

            if(company != null)
                company.Base64Logo = $"data:image/png;base64,{Convert.ToBase64String(company.Logo)}";

            return company;
        }

        public async Task<IEnumerable<CompanyViewModel>> GetCompanies(int userId)
        {
            IEnumerable<CompanyViewModel> companies = await _companyDAO.GetCompanies(userId);

            if (companies != null)
            {
                foreach (CompanyViewModel company in companies)
                {
                    company.Base64Logo = $"data:image/png;base64,{Convert.ToBase64String(company.Logo)}";
                }
            }

            return companies;
        }

        public async Task<bool> UpdateCompany(CompanyViewModel companyViewModel)
        {
            CompanyViewModel companyByOwnerId = await _companyDAO.GetCompany(companyViewModel.OwnerId);
            CompanyViewModel companyByName = await _companyDAO.GetCompany(companyViewModel.Name);

            if (!string.Equals(companyViewModel.Name, companyByOwnerId.Name, StringComparison.OrdinalIgnoreCase))
                if (companyByName != null) 
                    throw new Exception(_localizer["CompanyExists"]);

            string base64 = String.Empty;
            byte[] logo = new byte[0];

            if (!string.IsNullOrEmpty(companyViewModel.Base64Logo))
            {
                Regex regex = new Regex(@"^data:image/(jpeg|png);base64,", RegexOptions.IgnoreCase);
                base64 = regex.Replace(companyViewModel.Base64Logo, "");
                logo = Convert.FromBase64String(base64);
            }

            companyViewModel.Logo = logo;

            return await _companyDAO.UpdateCompany(companyViewModel);
        }

        public async Task<bool> DeleteCompany(int ownerId)
        {
            return await _companyDAO.DeleteCompany(ownerId);
        }

        public async Task<bool> CheckCompanyOwner(int? companyId, int? userId)
        {
            return await _companyDAO.CheckCompanyOwner(companyId, userId);
        }
    }
}
