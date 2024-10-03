using AttendanceSystem.ViewModels;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Service
{
    public interface ILeaveOpeningBalanceService:IService
    {
        Task<IEnumerable<EmployeeWithLeaveTypeList>> GetEmployeeLeaveTypeListAsync(EmployeeLeaveList model);
        Task<AccountResult> UpdateLeaveOpeningBalanceAsync(EmployeeWithYearLeaveList model,int CreatedBy);
        Task<List<SelectItemViewModel>> GetLeaveCodeList(int FiscalYear);
    }
}
