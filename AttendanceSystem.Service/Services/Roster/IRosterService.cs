using AttendanceSystem.ViewModels;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Services
{
   public interface IRosterService:IService
    {

        Task<IEnumerable<SelectItemIntViewModel>> GetShiftListAsync();

        #region Fixed Roster
        Task<IEnumerable<GroupedFixedRosterViewModel>> GetFixedRosterListAsync(FiscalWithEmployeeListViewModel model);
        AccountResult UpdateFixedRosterAsync(UpdateGroupedFixedRosterViewModel model);

        #endregion

        #region Weekly Roster
        Task<WeekWithEmployeeShiftList> GetWeeklyRosterListAsync(FiscalWithEmployeeListViewModel model);
        AccountResult UpdateWeeklyRosterAsync(UpdateGroupedWeeklyRosterViewModel model);
        #endregion

        #region Dynamic Roster
        Task<DaysInMonthWithEmployeeShiftList> GetDynamicRosterListAsync(FiscalWithEmployeeDynamicListViewModel model);
        AccountResult UpdateDynamicRosterAsync(DaysInMonthWithEmployeeShiftList model);


        #endregion

        #region Dynamic Roster Nepali
        Task<DaysInMonthWithEmployeeShiftList> GetDynamicRosterListNPAsync(FiscalWithEmployeeDynamicListViewModel model);
        AccountResult UpdateDynamicRosterNPAsync(DaysInMonthWithEmployeeShiftList model);
        #endregion
    }
}
