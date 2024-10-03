using Core.Service;
using System.Threading.Tasks;
using AttendanceSystem.Domains;
using AttendanceSystem.PageExtension;
using AttendanceSystem.ViewModels;
using System.Collections.Generic;

namespace AttendanceSystem.Services
{
    public interface ISectionService : IService
    {
        Task<IPagedList<SectionViewModel>> SectionListAsync(SectionSearchViewModel model);
        Task<AccountResult> InsertIntoSectionAsync(SectionViewModel model);
        Section GetSectionByID(int SectionID);
        Task<AccountResult> UpdateSectionAsync(SectionViewModel model);
        Task<AccountResult> DeleteSectionAsync(int SectionID);
        Task<SectionViewModel> GetSectionByIDAsync(int SectionID);
        Task<IList<SelectItemIntViewModel>> DDLSectionListAsync(int DepartmentID);
        Task<IList<SelectItemIntViewModel>> DDLDepartmentWiseSectionListAsync(DepartmentAndSectionIdModel model);
    }
}
