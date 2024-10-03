using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AttendanceSystem.AppSettings;
using AttendanceSystem.BaseController;
using AttendanceSystem.PageExtension;
using AttendanceSystem.Service;
using AttendanceSystem.Services;
using AttendanceSystem.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AttendanceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EmployeeController : BaseApiController
    {
        protected IEmployeeService _employeeService;
        private readonly JWTConfig _jwtSettings;
        protected ICommonService _commonService;
        public EmployeeController(IEmployeeService employeeService, IOptions<JWTConfig> jwtSettings, ICommonService commonService)
        {
            _employeeService = employeeService;
            _jwtSettings = jwtSettings.Value;
            _commonService = commonService;
        }

        [HttpPost("EmployeeListAsync")]
        public async Task<IActionResult> EmployeeListAsync([FromBody]EmployeeSearchViewModel model)
        {
            var list = await _employeeService.EmployeeListAsync(model);
            return Ok(list.ToJson());
        }


        [HttpPost("InsertIntoEmployeeAsync")]
        public async Task<IActionResult> InsertIntoEmployeeAsync([FromForm]EmployeeViewModel model)
        {
            model.CreatedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
            model.EmployeeImageFolerURL = _jwtSettings.EmployeeFolderURL;
            var result = await _employeeService.InsertIntoEmployeeAsync(model);
            return Ok(result);
        }

        [HttpPost("UpdateEmployeeAsync")]
        public async Task<IActionResult> UpdateEmployeeAsync([FromForm]EmployeeViewModel model)
        {
            model.ModifiedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
            model.EmployeeImageFolerURL = _jwtSettings.EmployeeFolderURL;
            var result = await _employeeService.UpdateEmployeeAsync(model);
            return Ok(result);
        }

        [HttpPost("DeleteEmployeeAsync/{EmployeeID}")]
        public async Task<IActionResult> DeleteEmployeeAsync(int EmployeeID)
        {
            var result = await _employeeService.DeleteEmployeeAsync(EmployeeID, _jwtSettings.EmployeeFolderURL);
            return Ok(result);
        }

        [HttpGet("GetEmployeeByIDAsync/{EmployeeID}")]
        public async Task<IActionResult> GetWorkShiftTypeByIDAsync(int EmployeeID)
        {
            var result = await _employeeService.GetEmployeeByIDAsync(EmployeeID,_jwtSettings.EmployeeFolderURL);
            return Ok(result);
        }
        [HttpGet("DDLEmployeeList")]
        public async Task<IActionResult> DDLEmployeeList()
        {
            var result = await _employeeService.DDLEmployeeListAsync();
            return Ok(result);
        }
        [HttpGet("DDLCheckClockStatusList")]
        public async Task<IActionResult> DDLCheckClockStatusList()
        {
            var result = await _commonService.GetCheckClockStatusList();
            return Ok(result);
        }
        [HttpGet("DDLDesignationWiseEmployeeList/{DesignationID}")]
        public async Task<IActionResult> DDLDesignationWiseEmployeeList(int DesignationID)
        {
            var result = await _employeeService.DDLDesignationWiseEmployeeListAsync(DesignationID);
            return Ok(result);
        }
        [HttpPost("DepartmentWiseEmployeeList")]
        public async Task<IActionResult> DepartmentWiseEmployeeList(DepartmentAndSectionIdModel model)
        {
            var result = await _employeeService.DepartmentWiseEmployeeListAsync(model);
            return Ok(result);
        }
        [HttpPost("DepartmentAndSectionWiseEmployeeList")]
        public async Task<IActionResult> DepartmentAndSectionWiseEmployeeList(DepartmentAndSectionIdModel model)
        {
            var result = await _employeeService.DepartmentAndSectionWiseEmployeeListAsync(model);
            return Ok(result);
        }
        [HttpPost("SectionWiseEmployeeList")]
        public async Task<IActionResult> SectionWiseEmployeeListAsync(DepartmentAndSectionIdModel model)
        {
            var result = await _employeeService.SectionWiseEmployeeListAsync(model);
            return Ok(result);
        }
        [HttpPost("LeaveApplicantEmployeeList")]
        public async Task<IActionResult> LeaveApplicantEmployeeList(SectionDesignationIdModel model)
        {
            var result = await _employeeService.LeaveApplicantEmployeeListAsync(model);
            return Ok(result);
        }
        [HttpPost("ManagerRoleEmployeeList")]
        public async Task<IActionResult> ManagerRoleEmployeeList(SectionDesignationIdModel model)
        {
            var result = await _employeeService.ManagerRoleEmployeeListAsync(model);
            return Ok(result);
        }
    }
    
}
