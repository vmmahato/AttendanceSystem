using Core.Service;
using System.Threading.Tasks;
using AttendanceSystem.Domains;
using AttendanceSystem.PageExtension;
using AttendanceSystem.ViewModels;
using System.Collections.Generic;

namespace AttendanceSystem.Services
{
    public interface ICompanyProfileService : IService
    {
        Task<IPagedList<CompanyProfileViewModel>> CompanyProfileListAsync(CompanyProfileSearchViewModel model);
        Task<AccountResult> InsertIntoCompanyProfileAsync(CompanyProfileViewModel model);
        CompanyProfile GetCompanyProfileByID(int CompanyProfileID);
        Task<AccountResult> UpdateCompanyProfileAsync(CompanyProfileViewModel model);
        Task<AccountResult> DeleteCompanyProfileAsync(int CompanyProfileID, string ImagePath);
        Task<CompanyProfileViewModel> GetCompanyProfileByIDAsync(int CompanyProfileID, string ImagePath);
        Task<IList<SelectItemIntViewModel>> DDLCompanyListAsync();
    }
}
