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

namespace AttendanceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class LeaveSettlementController : BaseApiController
    {
        protected ILeaveSettlementService _leaveSettlementService;

        public LeaveSettlementController(ILeaveSettlementService LeaveSettlementService)
        {
            _leaveSettlementService = LeaveSettlementService;
        }

        [HttpPost("LeaveSettlementList")]
        public async Task<IActionResult> LeaveSettlementList(LeaveSettlementSearchViewModel model)
        {
            
            var list = await _leaveSettlementService.LeaveSettlementListAsync(model);
            return Ok(list.ToJson());
        }


        [HttpPost("CreateLeaveSettlement")]
        public async Task<IActionResult> CreateLeaveSettlement(LeaveSettlementViewModel model)
        {
           model.CreatedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
            model.FiscalYear = CurrentUserDetails.FiscalYearID;
            var result= await _leaveSettlementService.InsertIntoLeaveSettlementAsync(model);
            return Ok(result);
        }

        [HttpPost("UpdateLeaveSettlement")]
        public async Task<IActionResult> UpdateLeaveSettlement(LeaveSettlementViewModel model)
        {
            model.ModifiedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
            model.FiscalYear = CurrentUserDetails.FiscalYearID;
            var result=await _leaveSettlementService.UpdateLeaveSettlementAsync(model);
            return Ok(result);
        }

        [HttpPost("DeleteLeaveSettlement/{SettlementID}")]
        public async Task<IActionResult> DeleteLeaveSettlement(int SettlementID)
        {
            var result=await _leaveSettlementService.DeleteLeaveSettlementAsync(SettlementID);
            return Ok(result);
        }

        [HttpGet("GetLeaveSettlementByID/{SettlementID}")]
        public async Task<IActionResult> GetLeaveSettlementByID(int SettlementID)
        {
          var result=  await _leaveSettlementService.GetLeaveSettlementByIDAsync(SettlementID);
            return Ok(result);
        }
        [HttpPost("GetEmployeeWiseLeaveSettlement")]
        public async Task<IActionResult> GetEmployeeWiseLeaveSettlement(SettlementIDsViewModel model)
        {
            var result = await _leaveSettlementService.GetEmployeeWiseLeaveSettlementAsync(model);
            return Ok(result);
        }
    }
    }