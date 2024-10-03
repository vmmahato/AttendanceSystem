using Core.Service;
using System.Threading.Tasks;
using AttendanceSystem.Domains;
using AttendanceSystem.PageExtension;
using AttendanceSystem.ViewModels;
using System.Collections.Generic;

namespace AttendanceSystem.Services
{
    public interface IFiscalYearService : IService
    {
        Task<IPagedList<FiscalYearViewModel>> FiscalYearListAsync(FiscalYearSearchViewModel model);
        Task<AccountResult> InsertIntoFiscalYearAsync(FiscalYearViewModel model);
        FiscalYears GetFiscalYearByID(int FiscalID);
        Task<AccountResult> UpdateFiscalYearAsync(FiscalYearViewModel model);
        Task<AccountResult> DeleteFiscalYearAsync(int FiscalID);
        Task<FiscalYearModel> GetFiscalYearByIDAsync(int FiscalID);
        Task<IList<SelectItemIntViewModel>> DDLFiscalYearListAsync();
    }
}
