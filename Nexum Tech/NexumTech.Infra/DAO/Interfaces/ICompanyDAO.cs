using NexumTech.Infra.Models;

namespace NexumTech.Infra.DAO.Interfaces
{
    public interface ICompanyDAO
    {
        public Task<bool> CreateCompany(CompanyViewModel companyViewModel);
        public Task<CompanyViewModel> GetCompany(int OwnerId);
        public Task<CompanyViewModel> GetCompany(string name);
        public Task<bool> UpdateCompany(CompanyViewModel companyViewModel);
        public Task<bool> DeleteCompany(int ownerId);
        public Task<bool> CheckCompanyOwner(int? companyId, int? userId);
        public Task<IEnumerable<CompanyViewModel>> GetCompanies(int userId);
    }
}
