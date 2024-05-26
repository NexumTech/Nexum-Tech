using NexumTech.Infra.Models;

namespace NexumTech.Domain.Interfaces
{
    public interface ICompanyService
    {
        public Task<bool> CreateCompany(CompanyViewModel companyViewModel);
        public Task<CompanyViewModel> GetCompany(int ownerId);
        public Task<bool> UpdateCompany(CompanyViewModel companyViewModel);
        public Task<bool> DeleteCompany(int ownerId);
        public Task<bool> CheckCompanyOwner(int? companyId, int? userId);
    }
}
