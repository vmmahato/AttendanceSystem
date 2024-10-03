using Core.Service;
using System.Threading.Tasks;
using AttendanceSystem.Domains;
using AttendanceSystem.PageExtension;
using AttendanceSystem.ViewModels;
using System.Collections.Generic;

namespace AttendanceSystem.Services
{
    public interface IDepartmentService : IService
    {
        Task<IPagedList<DepartmentViewModel>> DepartmentListAsync(DepartmentSearchViewModel model);
        Task<AccountResult> InsertIntoDepartmentAsync(DepartmentViewModel model);
        Department GetDepartmentByID(int DepartmentID);
        Task<AccountResult> UpdateDepartmentAsync(DepartmentViewModel model);
        Task<AccountResult> DeleteDepartmentAsync(int DepartmentID);
        Task<DepartmentViewModel> GetDepartmentByIDAsync(int DepartmentID);
        Task<IList<SelectItemIntViewModel>> DDLDepartmentListAsync();
    }
}
