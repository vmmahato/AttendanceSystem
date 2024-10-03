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

    public class LeaveSetupController : BaseApiController
    {
        protected ILeaveSetupService _LeaveSetupService;
        protected readonly BaseSetting _settings; //method to get value from appsettings.json
        private readonly JWTConfig _jwtSettings;

        public LeaveSetupController(ILeaveSetupService LeaveSetupService,
                              BaseSetting settings,
                              IOptions<JWTConfig> jwtSettings)
        {
            _LeaveSetupService = LeaveSetupService;
            _settings = settings;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("LeaveSetupList")]
        public async Task<IActionResult> LeaveSetupList(LeaveSetupSearchViewModel model)
        {
            
            var list = await _LeaveSetupService.LeaveSetupListAsync(model);
            return Ok(list.ToJson());
        }


        [HttpPost("CreateLeaveSetup")]
        public async Task<IActionResult> CreateLeaveSetup(LeaveSetupViewModel model)
        {
            if (CurrentUserDetails != null)
            {
                model.CreatedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
            }
            else
            {
                model.CreatedBy = 1;
            }
           model.FiscalYear = CurrentUserDetails.FiscalYearID;
           var result= await _LeaveSetupService.InsertIntoLeaveSetupAsync(model);
            return Ok(result);
        }

        [HttpPost("UpdateLeaveSetup")]
        public async Task<IActionResult> UpdateLeaveSetup(LeaveSetupViewModel model)
        {
            if (CurrentUserDetails != null)
            {
                model.ModifiedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
            }
            else
            {
                model.CreatedBy = 1;
            }
            model.FiscalYear = CurrentUserDetails.FiscalYearID;
            var result=await _LeaveSetupService.UpdateLeaveSetupAsync(model);
            return Ok(result);
        }

        [HttpPost("DeleteLeaveSetup/{LeaveID}")]
        public async Task<IActionResult> DeleteLeaveSetup(int LeaveID)
        {
            var result=await _LeaveSetupService.DeleteLeaveSetupAsync(LeaveID);
            return Ok(result);
        }

        [HttpGet("GetLeaveSetupByID/{LeaveID}")]
        public async Task<IActionResult> GetLeaveSetupByID(int LeaveID)
        {
          var result=  await _LeaveSetupService.GetLeaveSetupByIDAsync(LeaveID);
            return Ok(result);
        }
        [HttpGet("DDLLeaveTypeList")]
        public async Task<IActionResult> DDLLeaveTypeList()
        {
            var result = await _LeaveSetupService.DDLLeaveTypeListAsync();
            return Ok(result);
        }
        [HttpGet("DDLGenderWiseLeaveTypeList/{EmployeeID}")]
        public async Task<IActionResult> DDLGenderWiseLeaveTypeList(int EmployeeID)
        {
            var result = await _LeaveSetupService.DDLGenderWiseLeaveTypeListAsync(EmployeeID);
            return Ok(result);
        }
        [HttpPost("PullDefaultLeave")]
        public async Task<IActionResult> PullDefaultLeave()
        {
            var result = await _LeaveSetupService.PullDefaultLeaveAsync(CurrentUserDetails.FiscalYearID);
            return Ok(result);
        }
        
    }
    }