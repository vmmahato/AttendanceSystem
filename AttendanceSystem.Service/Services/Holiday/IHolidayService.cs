using Core.Service;
using System.Threading.Tasks;
using AttendanceSystem.Domains;
using AttendanceSystem.PageExtension;
using AttendanceSystem.ViewModels;
using System.Collections.Generic;

namespace AttendanceSystem.Services
{
    public interface IHolidayService : IService
    {
        Task<IPagedList<HolidayViewModel>> HolidayListAsync(HolidaySearchViewModel model);
        Task<AccountResult> InsertIntoHolidayAsync(HolidayViewModel model);
        Holiday GetHolidayByID(int HolidayID);
        Task<AccountResult> UpdateHolidayAsync(HolidayViewModel model);
        Task<AccountResult> DeleteHolidayAsync(int HolidayID);
        Task<HolidayModel> GetHolidayByIDAsync(int HolidayID);
    }
}
