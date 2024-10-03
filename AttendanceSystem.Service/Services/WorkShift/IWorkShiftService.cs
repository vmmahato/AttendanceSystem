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
    public interface IWorkShiftService: IService
    {
        Task<IPagedList<WorkShiftListViewModel>> WorkShiftListAsync(WorkShiftSearchViewModel model);
        Task<AccountResult> InsertIntoWorkShiftAsync(WorkShiftViewModel model);
        WorkShift GetWorkShiftByID(int WorkShiftID);
        Task<AccountResult> UpdateWorkShiftAsync(WorkShiftViewModel model);
        Task<AccountResult> DeleteWorkShiftAsync(int WorkShiftID);
        Task<WorkShiftListViewModel> GetWorkShiftByIDAsync(int WorkShiftID);
        Task<IEnumerable<GetWorkShiftTypeViewModel>> GetWorkShiftTypeByIDAsync(int WorkShiftID);
    }
}
