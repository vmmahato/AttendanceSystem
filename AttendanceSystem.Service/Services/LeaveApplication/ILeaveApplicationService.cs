using Core.Service;
using System.Threading.Tasks;
using AttendanceSystem.Domains;
using AttendanceSystem.PageExtension;
using AttendanceSystem.ViewModels;
using System.Collections;
using System.Collections.Generic;

namespace AttendanceSystem.Services
{
    public interface ILeaveApplicationService : IService
    {
        #region Leave Application
        Task<IPagedList<LeaveApplicationViewModel>> LeaveApplicationListAsync(LeaveApplicationSearchViewModel model);
        Task<AccountResult> InsertIntoLeaveApplicationAsync(LeaveApplicationViewModel model);
        LeaveApplication GetLeaveApplicationByID(int LeaveApplicationID);
        Task<AccountResult> UpdateLeaveApplicationAsync(LeaveApplicationViewModel model);
        Task<AccountResult> DeleteLeaveApplicationAsync(int LeaveApplicationID);
        Task<LeaveApplicationViewModel> GetLeaveApplicationByIDAsync(int LeaveApplicationID);
        Task<LeaveApplicationViewModel> GetLeaveApplicationViewByIDAsync(int LeaveApplicationID);
        #endregion

        #region Leave Approve
        Task<IPagedList<LeaveApplicationViewModel>> GetPendingLeaveApplicationListAsync(LeaveApplicationSearchViewModel model);
        Task<AccountResult> UpdatePendingLeaveApplicationAsync(LeaveApplicationViewModel model);
        #endregion
        Task<RemainingLeaveViewModel> GetEmplyeeOTAsync(LeaveAppicantViewModel model);
        Task<RemainingLeaveViewModel> GetEmplyeeRemainingLeaveAsync(LeaveAppicantViewModel model);
        Task<GenericResult<IEnumerable<GroupEmployeeWithShiftByEmployeeIDViewModel>>> GetReplacementEmployeeList(ReplacementEmployeeViewModel model);
    }
}
