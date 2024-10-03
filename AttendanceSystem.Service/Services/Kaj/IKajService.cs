using AttendanceSystem.Domains;
using AttendanceSystem.PageExtension;
using AttendanceSystem.ViewModels;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Services
{
    public interface IKajService: IService
    {
        Task<IPagedList<KajModel>> KajListAsync(KajSearchViewModel model);
        Task<AccountResult> InsertIntoKajAsync(KajViewModel model);
        Kaj GetKajByID(int KajID);
        Task<AccountResult> UpdateKajAsync(KajViewModel model);
        Task<AccountResult> DeleteKajAsync(int KajID);
        Task<KajModel> GetKajByIDAsync(int KajID);
        Task<IPagedList<KajModel>> GetPendingKajListAsync(KajSearchViewModel model);
        Task<AccountResult> UpdatePendingKajAsync(KajViewModel model);
    }
}
