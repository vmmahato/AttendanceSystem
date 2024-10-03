using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AttendanceSystem.BaseController;
using AttendanceSystem.PageExtension;
using AttendanceSystem.Service;
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
    public class WorkShiftController : BaseApiController
    {
        protected IWorkShiftService _workShiftService;
        public WorkShiftController(IWorkShiftService workShiftService)
        {
            _workShiftService = workShiftService;
        }

        [HttpPost("WorkShiftListAsync")]
        public async Task<IActionResult> WorkShiftListAsync([FromBody]WorkShiftSearchViewModel model)
        {
            var list = await _workShiftService.WorkShiftListAsync(model);
            return Ok(list.ToJson());
        }


        [HttpPost("InsertIntoWorkShiftAsync")]
        public async Task<IActionResult> InsertIntoWorkShiftAsync([FromBody]WorkShiftViewModel model)
        {
            model.CreatedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
            var result = await _workShiftService.InsertIntoWorkShiftAsync(model);
            return Ok(result);
        }

        [HttpPost("UpdateWorkShiftAsync")]
        public async Task<IActionResult> UpdateWorkShiftAsync([FromBody]WorkShiftViewModel model)
        {
            model.ModifiedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
            var result = await _workShiftService.UpdateWorkShiftAsync(model);
            return Ok(result);
        }

        [HttpPost("DeleteWorkShiftAsync/{WorkShiftID}")]
        public async Task<IActionResult> DeleteWorkShiftAsync(int WorkShiftID)
        {
            var result = await _workShiftService.DeleteWorkShiftAsync(WorkShiftID);
            return Ok(result);
        }

        [HttpGet("GetWorkShiftTypeByIDAsync/{WorkShiftID}")]
        public async Task<IActionResult> GetWorkShiftTypeByIDAsync(int WorkShiftID)
        {
            var workShift = await _workShiftService.GetWorkShiftByIDAsync(WorkShiftID);
            var workShiftType = await _workShiftService.GetWorkShiftTypeByIDAsync(WorkShiftID);
            return Ok(new { workShift = workShift, workShiftType = workShiftType });
        }
    }
}