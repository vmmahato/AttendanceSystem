using AttendanceSystem.Domains;
using AttendanceSystem.PageExtension;
using AttendanceSystem.ViewModels;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Service
{
  public interface IEmployeeService:IService
    {
        Task<IPagedList<EmployeeModel>> EmployeeListAsync(EmployeeSearchViewModel model);
        Task<AccountResult> InsertIntoEmployeeAsync(EmployeeViewModel model);
        Employee GetEmployeeByID(int EmployeeID);
        Task<AccountResult> UpdateEmployeeAsync(EmployeeViewModel model);
        Task<AccountResult> DeleteEmployeeAsync(int EmployeeID,string ImagePath);
        Task<EmployeeModel> GetEmployeeByIDAsync(int EmployeeID,string ImagePath);
        Task<IList<SelectItemIntViewModel>> DDLEmployeeListAsync();
        Task<IList<SelectItemIntViewModel>> DDLDesignationWiseEmployeeListAsync(int DesignationID);
        Task<IList<SelectItemIntViewModel>> DepartmentWiseEmployeeListAsync(DepartmentAndSectionIdModel model);
        Task<IList<SelectItemIntViewModel>> SectionWiseEmployeeListAsync(DepartmentAndSectionIdModel model);
        Task<IList<SelectItemsIntViewModel>> DepartmentAndSectionWiseEmployeeListAsync(DepartmentAndSectionIdModel model);
        Task<IList<SelectItemIntViewModel>> LeaveApplicantEmployeeListAsync(SectionDesignationIdModel model);
        Task<IList<SelectItemIntViewModel>> ManagerRoleEmployeeListAsync(SectionDesignationIdModel model);
    }
}
