using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AttendanceSystem.BaseController;
using AttendanceSystem.Services;
using AttendanceSystem.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RosterController : BaseApiController
    {
        protected IRosterService _rosterService;
        public RosterController(IRosterService rosterService)
        {
            _rosterService = rosterService;
        }

        #region Fixed Roster

        [HttpPost("GetFixedRosterListAsync")]
        public async Task<IActionResult> GetFixedRosterListAsync([FromBody]FiscalWithEmployeeListViewModel model)
        {
            model.FiscalYearID = CurrentUserDetails.FiscalYearID;
            var result = await _rosterService.GetFixedRosterListAsync(model);
            return Ok(result);
        }

        [HttpPost("UpdateFixedRosterAsync")]
        public IActionResult UpdateFixedRosterAsync([FromBody]UpdateGroupedFixedRosterViewModel model)
        {
            model.FiscalYear = CurrentUserDetails.FiscalYearID;
            model.CreatedBy = CurrentUserDetails.EmployeeID;
            var result = _rosterService.UpdateFixedRosterAsync(model);
            return Ok(result);
        }

        #endregion

        #region WeeklyRoster

        [HttpPost("GetWeeklyRosterListAsync")]
        public async Task<IActionResult> GetWeeklyRosterListAsync([FromBody]FiscalWithEmployeeListViewModel model)
        {
            model.FiscalYearID = CurrentUserDetails.FiscalYearID;
            var result = await _rosterService.GetWeeklyRosterListAsync(model);
            return Ok(result);
        }

        [HttpPost("UpdateWeeklyRosterAsync")]
        public IActionResult UpdateWeeklyRosterAsync([FromBody]UpdateGroupedWeeklyRosterViewModel model)
        {
            model.FiscalYear = CurrentUserDetails.FiscalYearID;
            model.CreatedBy = CurrentUserDetails.EmployeeID;
            var result = _rosterService.UpdateWeeklyRosterAsync(model);
            return Ok(result);
        }

        #endregion

        #region Dynamic Roster

        [HttpPost("GetDynamicRosterListAsync")]
        public async Task<IActionResult> GetDynamicRosterListAsync([FromBody]FiscalWithEmployeeDynamicListViewModel model)
        {
            var result = await _rosterService.GetDynamicRosterListAsync(model);
            return Ok(result);
        }

        [HttpPost("UpdateDynamicRosterAsync")]
        public IActionResult UpdateDynamicRosterAsync([FromBody]DaysInMonthWithEmployeeShiftList model)
        {
            model.CreatedBy = CurrentUserDetails.EmployeeID;
            var result = _rosterService.UpdateDynamicRosterAsync(model);
            return Ok(result);
        }

        #endregion

        [HttpGet("GetShiftListAsync")]
        public async Task<IActionResult> GetShiftListAsync()
        {
            var result = await _rosterService.GetShiftListAsync();
            return Ok(result);
        }

        #region Dynamic Roster Nepali
        [HttpPost("GetDynamicRosterListNPAsync")]
        public async Task<IActionResult> GetDynamicRosterListNPAsync([FromBody]FiscalWithEmployeeDynamicListViewModel model)
        {
            var result = await _rosterService.GetDynamicRosterListNPAsync(model);
            return Ok(result);
        }

        [HttpPost("UpdateDynamicRosterNPAsync")]
        public IActionResult UpdateDynamicRosterNPAsync([FromBody]DaysInMonthWithEmployeeShiftList model)
        {
            model.CreatedBy = CurrentUserDetails.EmployeeID;
            var result = _rosterService.UpdateDynamicRosterNPAsync(model);
            return Ok(result);
        }
        #endregion
    }
}