using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AttendanceSystem.BaseController;
using AttendanceSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SharedController : BaseApiController
    {
        protected ICommonService _commonService;
        public SharedController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        [HttpGet("GetGenderList")]
        public async Task<IActionResult> GetGenderList()
        {
            var result = await _commonService.GetGenderList();
            return Ok(result);
        }

        [HttpGet("GetHolidayTypeList")]
        public async Task<IActionResult> GetHolidayTypeList()
        {
            var result = await _commonService.GetHolidayTypeList();
            return Ok(result);
        }

        [HttpGet("GetApplicableReligionList")]
        public async Task<IActionResult> GetApplicableReligionList()
        {
            var result = await _commonService.GetApplicableReligionList();
            return Ok(result);
        }

        
        [HttpGet("GetAttendanceLogCount")]
        public async Task<IActionResult> GetAttendanceLogCount()
        {
            var result = await _commonService.GetAttendanceLogCount();
            return Ok(result);
        }

        [HttpGet("GetCounts")]
        public async Task<IActionResult> GetCounts()
        {
            var result = await _commonService.GetCounts();
            return Ok(result);
        }

        [HttpGet("GetAttendanceStatusCount")]
        public async Task<IActionResult> GetAttendanceStatusCount()
        {
            var result = await _commonService.GetAttendanceStatusCount();
            return Ok(result);
        } 
        
        [HttpGet("GetDeviceStatusCount")]
        public async Task<IActionResult> GetDeviceStatusCount()
        {
            var result = await _commonService.GetDeviceStatusCount();
            return Ok(result);
        }

        [HttpGet("GetLeaveCount")]
        public async Task<IActionResult> GetLeaveCount()
        {
            var result = await _commonService.GetLeaveCount();
            return Ok(result);
        }

        [HttpGet("GetAbsentCount")]
        public async Task<IActionResult> GetAbsentCount()
        {
            var result = await _commonService.GetAbsentCount(CurrentUserDetails.FiscalYearID);
            return Ok(result);
        }

        [HttpGet("GetHolidayCount")]
        public async Task<IActionResult> GetHolidayCount()
        {
            var result = await _commonService.GetHolidayCount();
            return Ok(result);
        }

        [HttpGet("GetVisitorCount")]
        public async Task<IActionResult> GetVisitorCount()
        {
            var result = await _commonService.GetVisitorCount();
            return Ok(result);
        }

        [HttpGet("GetStatus")]
        public async Task<IActionResult> GetStatus()
        {
            var result = await _commonService.GetStatus(CurrentUserDetails.FiscalYearID);
            return Ok(result);
        }

    }
}