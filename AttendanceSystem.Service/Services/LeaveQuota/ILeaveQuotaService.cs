using Core.Service;
using System.Threading.Tasks;
using AttendanceSystem.Domains;
using AttendanceSystem.PageExtension;
using AttendanceSystem.ViewModels;
using System.Collections.Generic;

namespace AttendanceSystem.Services
{
    public interface ILeaveQuotaService : IService
    {
        Task<IEnumerable<LeaveQuotaViewModel>> LeaveQuotaListAsync(int DesignationID);
        Task<AccountResult> InsertIntoLeaveQuotaAsync(List<LeaveQuotaViewModel> model,int UserID,int FiscalYear);
        LeaveQuota GetLeaveQuotaByID(int LeaveQuotaID, int DesignationID);
    }
}
