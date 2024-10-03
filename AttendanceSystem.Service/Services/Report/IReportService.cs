using Core.Service;
using System.Threading.Tasks;
using AttendanceSystem.ViewModels;
using System.Collections.Generic;

namespace AttendanceSystem.Services
{
    public interface IReportService : IService
    {
        Task<IEnumerable<DailyReportViewModel>> DailyAttendanceReportAsync(ReportSearchViewModel model);
        Task<IEnumerable<MonthlyReportViewModel>> MonthlyAttendanceReportAsync(ReportSearchViewModel model);
        Task<RosterReportModel> GetRosterReportListAsync(ReportSearchViewModel model);
        Task<RosterReportModel> GetRosterDateWiseReportListAsync(ReportSearchViewModel model);
        Task<IEnumerable<DailyLeaveReportViewModel>> DailyLeaveReportAsync(ReportSearchViewModel model);
        Task<IEnumerable<DailyManualReportViewModel>> DailyManualPuntchReportAsync(ReportSearchViewModel model);
        Task<IEnumerable<DailyOfficeVisitReportViewModel>> DailyOfficeVisitReportAsync(ReportSearchViewModel model);
        Task<IEnumerable<MonthlyManualReportViewModel>> MonthlyManualPuntchReportAsync(ReportSearchViewModel model);

        Task<IEnumerable<DailyOfficeVisitReportViewModel>> MonthlyOfficeVisitReportAsync(ReportSearchViewModel model);

        Task<IEnumerable<DailyLeaveReportViewModel>> MonthlyLeaveReportAsync(ReportSearchViewModel model);
        Task<IEnumerable<MissingpunchReportViewModel>> DailyMissingpunchReportAsync(ReportSearchViewModel model);
        Task<IEnumerable<MissingpunchReportViewModel>> MonthlyMissingpunchReportAsync(ReportSearchViewModel model);

    }
}
