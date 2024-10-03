using Core.Service;
using System.Threading.Tasks;
using AttendanceSystem.Domains;
using AttendanceSystem.PageExtension;
using AttendanceSystem.ViewModels;
using System.Collections.Generic;

namespace AttendanceSystem.Services
{
    public interface IDeviceService : IService
    {
        Task<IPagedList<DeviceViewModel>> DeviceListAsync(DeviceSearchViewModel model);
        Task<AccountResult> InsertIntoDeviceAsync(DeviceViewModel model);
        Device GetDeviceByID(int DeviceID);
        Task<AccountResult> UpdateDeviceAsync(DeviceViewModel model);
        Task<AccountResult> DeleteDeviceAsync(int DeviceID);
        Task<DeviceViewModel> GetDeviceByIDAsync(int DeviceID);
        Task<List<DeviceViewModel>> GetDeviceDetailsWithStatus();
        Task<IList<SelectItemIntViewModel>> DDLDeviceListAsync();
        Task<IList<SelectItemIntViewModel>> DDLDeviceModelListAsync();
    }
}
