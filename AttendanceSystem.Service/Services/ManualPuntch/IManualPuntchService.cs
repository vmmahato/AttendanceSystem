using Core.Service;
using System.Threading.Tasks;
using AttendanceSystem.Domains;
using AttendanceSystem.PageExtension;
using AttendanceSystem.ViewModels;
using System.Collections.Generic;
using System;

namespace AttendanceSystem.Services
{
    public interface IManualPuntchService : IService
    {
        Task<IPagedList<ManualPuntchViewModel>> ManualPuntchListAsync(ManualPuntchSearchViewModel model);

        Task<AccountResult> InsertIntoManualPuntchAsync(ManualPuntchViewModelResult model);
        Task<AccountResult> UpdateManualPuntchAsync(ManualPuntchViewModelResult model);
        Task<AccountResult> DeleteManualPuntchAsync(Guid ManualPuntchID);
        DeviceLogs GetDevicelogsByID(Guid DevicelogsID);
        Task<ManualPuntchModel> GetManualPuntchByDevicelogsIDAsync(Guid DevicelogsID);

    }
}
