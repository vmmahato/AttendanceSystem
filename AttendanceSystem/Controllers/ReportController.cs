using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AttendanceSystem.BaseController;
using AttendanceSystem.Services;
using AttendanceSystem.ViewModels;

namespace AttendanceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ReportController : BaseApiController
    {
        protected IReportService _ReportService;

        public ReportController(IReportService ReportService)
        {
            _ReportService = ReportService;
        }

        [HttpPost("DailyAttendanceReport")]
        public async Task<IActionResult> DailyReportList(ReportSearchViewModel model)
        {
            var list = await _ReportService.DailyAttendanceReportAsync(model);
            return Ok(list);
        }
        [HttpPost("MonthlyAttendanceReport")]
        public async Task<IActionResult> MonthlyReportList(ReportSearchViewModel model)
        {
            var list = await _ReportService.MonthlyAttendanceReportAsync(model);
            return Ok(list);
        }
        [HttpPost("RosterReportList")]
        public async Task<IActionResult> GetRosterReportList(ReportSearchViewModel model)
        {
            model.FiscalYearID = CurrentUserDetails.FiscalYearID;
            var list = await _ReportService.GetRosterReportListAsync(model);
            return Ok(list);
        }
        [HttpPost("RosterDatewiseReportList")]
        public async Task<IActionResult> RosterDatewiseReportList(ReportSearchViewModel model)
        {
            model.FiscalYearID = CurrentUserDetails.FiscalYearID;
            var list = await _ReportService.GetRosterDateWiseReportListAsync(model);
            return Ok(list);
        }

        [HttpPost("DailyLeaveReport")]
        public async Task<IActionResult> DailyLeaveReportList(ReportSearchViewModel model)
        {
            var list = await _ReportService.DailyLeaveReportAsync(model);
            return Ok(list);
        }

        [HttpPost("DailyManualPuntchReport")]

        public async Task<IActionResult> DailyManualPuntchReportList(ReportSearchViewModel model)
        {
            var list = await _ReportService.DailyManualPuntchReportAsync(model);
            return Ok(list);
        }

        [HttpPost("DailyOfficeVisitReport")]
        public async Task<IActionResult> DailyOfficeVisitReportList(ReportSearchViewModel model)
        {
            var list = await _ReportService.DailyOfficeVisitReportAsync(model);
            return Ok(list);
        }

        [HttpPost("MonthlyManualPuntchReport")]
        public async Task<IActionResult> MonthlyManualPuntchReportList(ReportSearchViewModel model)
        {
            var list = await _ReportService.MonthlyManualPuntchReportAsync(model);
            return Ok(list);
        }

        [HttpPost("MonthlyOfficeVisitReport")]
        public async Task<IActionResult> MonthlyOfficeVisitReportList(ReportSearchViewModel model)
        {
            var list = await _ReportService.MonthlyOfficeVisitReportAsync(model);
            return Ok(list);
        }

        [HttpPost("MonthlyLeaveReport")]
        public async Task<IActionResult> MonthlyLeaveReportList(ReportSearchViewModel model)
        {
            var list = await _ReportService.MonthlyLeaveReportAsync(model);
            return Ok(list);
        }

        [HttpPost("DailyMissingPunchReport")]
        public async Task<IActionResult> DailyMissingReportList(ReportSearchViewModel model)
        {
            var list = await _ReportService.DailyMissingpunchReportAsync(model);
            return Ok(list);
        }

        [HttpPost("MonthlyMissingPunchReport")]
        public async Task<IActionResult> MonthlyMissingReportList(ReportSearchViewModel model)
        {
            var list = await _ReportService.MonthlyMissingpunchReportAsync(model);
            return Ok(list);
        }
    }
}