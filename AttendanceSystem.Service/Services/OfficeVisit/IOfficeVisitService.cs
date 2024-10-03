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
    public interface IOfficeVisitService: IService
    {
        Task<IPagedList<OfficeVisitModel>> OfficeVisitListAsync(OfficeVisitSearchViewModel model);
        Task<AccountResult> InsertIntoOfficeVisitAsync(OfficeVisitViewModel model);
        OfficeVisit GetOfficeVisitByID(int OfficeVisitID);
        Task<AccountResult> UpdateOfficeVisitAsync(OfficeVisitViewModel model);
        Task<AccountResult> DeleteOfficeVisitAsync(int OfficeVisitID);
        Task<OfficeVisitModel> GetOfficeVisitByIDAsync(int OfficeVisitID);
        #region officevisit Approval
        Task<IPagedList<OfficeVisitModel>> GetPendingOfficeVisitListAsync(OfficeVisitSearchViewModel model);
        Task<AccountResult> UpdatePendingOfficeVisitAsync(OfficeVisitModel model);
        #endregion
    }
}
