using Core.Service;
using System.Threading.Tasks;
using AttendanceSystem.Domains;
using AttendanceSystem.PageExtension;
using AttendanceSystem.ViewModels;
using System.Collections.Generic;

namespace AttendanceSystem.Services
{
    public interface IDesignationService : IService
    {
        Task<IPagedList<DesignationViewModel>> DesignationListAsync(DesignationSearchViewModel model);
        Task<AccountResult> InsertIntoDesignationAsync(DesignationViewModel model);
        Designation GetDesignationByID(int DesignationID);
        Task<AccountResult> UpdateDesignationAsync(DesignationViewModel model);
        Task<AccountResult> DeleteDesignationAsync(int DesignationID);
        Task<DesignationViewModel> GetDesignationByIDAsync(int DesignationID);
        Task<IList<SelectItemIntViewModel>> DDLDesignationListAsync();
    }
}
