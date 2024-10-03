using Core.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AttendanceSystem.ViewModels;
using Microsoft.AspNetCore.Http;

namespace AttendanceSystem.Services
{
  public  interface ICommonService:IService
    {
        Task<IEnumerable<SelectItemViewModel>> GetGenderList();
        string GetFullPath(string URL, string BranchIDFromServer, string Branch);
        Task<ImageTypesViewModel> SaveFiles(List<FileUploadViewModel> files, string DirectoryPath);
        List<FileUploadViewModel> GetLogo(LogoViewModel model);
        List<string> CheckFiles(IFormFile File);
        List<string> CheckLogo(LogoViewModel model);
        void DeleteFiles(int ID, string Type);
        Task<IEnumerable<SelectItemViewModel>> GetReportYearList();
        Task<SelectItemViewModel> GetReportYearList(int Year);
        Task<IEnumerable<SelectItemViewModel>> GetDesignationList();
        Task<IEnumerable<SelectItemViewModel>> TestCodeTable();
        string GetFullPath(string URL);
        List<FileUploadViewModel> GetFiles(IFormFile File, string ImageType);
        void DeleteFilesFromDirectoryPath(string DirectoryPath);
        Task<IEnumerable<SelectItemViewModel>> GetCheckClockStatusList();
        Task<IEnumerable<SelectItemViewModel>> GetHolidayTypeList();
        Task<IEnumerable<SelectItemViewModel>> GetApplicableReligionList();

        //DashBoard
        Task<IEnumerable<DashboardLeaveViewModel>> GetLeaveCount();
        Task<DashboardAbsentCountWithViewModel> GetAbsentCount(int FiscalYear);
        Task<IEnumerable<DashboardHolidayViewModel>> GetHolidayCount();
        Task<IEnumerable<DashboardVisitorViewModel>> GetVisitorCount();
        Task<DashboardCounts> GetCounts();
        Task<IEnumerable<DashboardAttendanceLogViewModel>> GetAttendanceLogCount();
        Task<IEnumerable<DashboardAttendanceStatusViewModel>> GetAttendanceStatusCount();
        Task<StatusViewModel> GetStatus(int FiscalYear);
        Task<IEnumerable<DashboardDeviceStatusViewModel>> GetDeviceStatusCount();
    }
}
