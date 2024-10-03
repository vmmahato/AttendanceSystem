using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using AttendanceSystem.AppSettings;
using AttendanceSystem.BaseController;
using AttendanceSystem.Database.Configuration;
using AttendanceSystem.PageExtension;
using AttendanceSystem.Services;
using AttendanceSystem.ViewModels;
using AttendanceSystem.Domains;

namespace AttendanceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class LeaveApplicationController : BaseApiController
    {
        protected ILeaveApplicationService _leaveApplicationService;

        public LeaveApplicationController(ILeaveApplicationService leaveApplicationService)
        {
            _leaveApplicationService = leaveApplicationService;
        }
        #region Leave Application
        [HttpPost("LeaveApplicationList")]
        public async Task<IActionResult> LeaveApplicationList(LeaveApplicationSearchViewModel model)
        {
            
            var list = await _leaveApplicationService.LeaveApplicationListAsync(model);
            return Ok(list.ToJson());
        }


        [HttpPost("CreateLeaveApplication")]
        public async Task<IActionResult> CreateLeaveApplication(LeaveApplicationViewModel model)
        {
            model.CreatedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
            model.FiscalYear = CurrentUserDetails.FiscalYearID;
            var result= await _leaveApplicationService.InsertIntoLeaveApplicationAsync(model);
            return Ok(result);
        }

        [HttpPost("UpdateLeaveApplication")]
        public async Task<IActionResult> UpdateLeaveApplication(LeaveApplicationViewModel model)
        {
            model.ModifiedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
            model.FiscalYear = CurrentUserDetails.FiscalYearID;
            var result=await _leaveApplicationService.UpdateLeaveApplicationAsync(model);
            return Ok(result);
        }

        [HttpPost("DeleteLeaveApplication/{LeaveApplicationID}")]
        public async Task<IActionResult> DeleteLeaveApplication(int LeaveApplicationID)
        {
            var result=await _leaveApplicationService.DeleteLeaveApplicationAsync(LeaveApplicationID);
            return Ok(result);
        }

        [HttpGet("GetLeaveApplicationByID/{LeaveApplicationID}")]
        public async Task<IActionResult> GetLeaveApplicationByID(int LeaveApplicationID)
        {
          var result=  await _leaveApplicationService.GetLeaveApplicationByIDAsync(LeaveApplicationID);
            return Ok(result);
        }

        [HttpGet("GetLeaveApplicationViewByIDAsync/{LeaveApplicationID}")]
        public async Task<IActionResult> GetLeaveApplicationViewByIDAsync(int LeaveApplicationID)
        {
            var result = await _leaveApplicationService.GetLeaveApplicationViewByIDAsync(LeaveApplicationID);
            if (result != null)
            {
                result.FromDateString = result.FromDate.ToString("yyyy-MM-dd");
                result.ToDateString= result.ToDate.ToString("yyyy-MM-dd");
            }
            return Ok(result);
        }
        #endregion
        #region Leave Approve
        [HttpPost("GetPendingLeaveApplicationList")]
        public async Task<IActionResult> GetPendingLeaveApplicationList(LeaveApplicationSearchViewModel model)
        {
            var list = await _leaveApplicationService.GetPendingLeaveApplicationListAsync(model);
            return Ok(list.ToJson());
        }
        [HttpPost("UpdatePendingLeaveApplication")]
        public async Task<IActionResult> UpdatePendingLeaveApplication(LeaveApplicationViewModel model)
        {
            model.ModifiedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
            var result = await _leaveApplicationService.UpdatePendingLeaveApplicationAsync(model);
            return Ok(result);
        }
        #endregion

        [HttpPost("GetReplacementEmployeeList")]
        public async Task<IActionResult> GetReplacementEmployeeList([FromBody] ReplacementEmployeeViewModel model)
        {
            model.FiscalYearID = CurrentUserDetails.FiscalYearID;
            var result = await _leaveApplicationService.GetReplacementEmployeeList(model);
            return Ok(result);
        }
        [HttpPost("GetEmplyeeOT")]
        public async Task<IActionResult> GetEmplyeeOT(LeaveAppicantViewModel model)
        {
            model.FiscalYear = CurrentUserDetails.FiscalYearID;
            var result = await _leaveApplicationService.GetEmplyeeOTAsync(model);
            return Ok(result);
        }
        [HttpPost("GetEmplyeeRemainingLeave")]
        public async Task<IActionResult> GetEmplyeeRemainingLeave(LeaveAppicantViewModel model)
        {
            model.FiscalYear = CurrentUserDetails.FiscalYearID;
            var result = await _leaveApplicationService.GetEmplyeeRemainingLeaveAsync(model);
            return Ok(result);
        }
    }
}