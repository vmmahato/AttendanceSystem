using Core.Service;
using System.Threading.Tasks;
using AttendanceSystem.Domains;
using AttendanceSystem.PageExtension;
using AttendanceSystem.ViewModels;
using System.Collections.Generic;

namespace AttendanceSystem.Services
{
    public interface ILeaveSettlementService : IService
    {
        Task<IPagedList<LeaveSettlementViewModel>> LeaveSettlementListAsync(LeaveSettlementSearchViewModel model);
        Task<AccountResult> InsertIntoLeaveSettlementAsync(LeaveSettlementViewModel model);
        LeaveSettlement GetLeaveSettlementByID(int LeaveSettlementID);
        Task<AccountResult> UpdateLeaveSettlementAsync(LeaveSettlementViewModel model);
        Task<AccountResult> DeleteLeaveSettlementAsync(int LeaveSettlementID);
        Task<LeaveSettlementViewModel> GetLeaveSettlementByIDAsync(int LeaveSettlementID);
        Task<SettlementViewModel> GetEmployeeWiseLeaveSettlementAsync(SettlementIDsViewModel model);
    }
}
