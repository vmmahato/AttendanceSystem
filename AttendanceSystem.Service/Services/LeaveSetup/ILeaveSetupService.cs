using Core.Service;
using System.Threading.Tasks;
using AttendanceSystem.Domains;
using AttendanceSystem.PageExtension;
using AttendanceSystem.ViewModels;
using System.Collections.Generic;

namespace AttendanceSystem.Services
{
    public interface ILeaveSetupService : IService
    {
        Task<IPagedList<LeaveSetupViewModel>> LeaveSetupListAsync(LeaveSetupSearchViewModel model);
        Task<AccountResult> InsertIntoLeaveSetupAsync(LeaveSetupViewModel model);
        LeaveSetup GetLeaveSetupByID(int LeaveID);
        Task<AccountResult> UpdateLeaveSetupAsync(LeaveSetupViewModel model);
        Task<AccountResult> DeleteLeaveSetupAsync(int LeaveID);
        Task<LeaveSetupViewModel> GetLeaveSetupByIDAsync(int LeaveID);
        Task<IList<SelectItemIntViewModel>> DDLLeaveTypeListAsync();
        Task<IList<SelectItemIntViewModel>> DDLGenderWiseLeaveTypeListAsync(int EmployeeID);
        Task<AccountResult> PullDefaultLeaveAsync(int FiscalYear);
    }
}
