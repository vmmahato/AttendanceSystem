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

    public class HolidayController : BaseApiController
    {
        protected IHolidayService _HolidayService;

        public HolidayController(IHolidayService HolidayService)
        {
            _HolidayService = HolidayService;
        }

        [HttpPost("HolidayList")]
        public async Task<IActionResult> HolidayList(HolidaySearchViewModel model)
        {
            var list = await _HolidayService.HolidayListAsync(model);
            return Ok(list.ToJson());
        }


        [HttpPost("CreateHoliday")]
        public async Task<IActionResult> CreateHoliday(HolidayViewModel model)
        {
           model.CreatedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
           var result= await _HolidayService.InsertIntoHolidayAsync(model);
           return Ok(result);
        }

        [HttpPost("UpdateHoliday")]
        public async Task<IActionResult> UpdateHoliday(HolidayViewModel model)
        {
            model.ModifiedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
            var result=await _HolidayService.UpdateHolidayAsync(model);
            return Ok(result);
        }

        [HttpPost("DeleteHoliday/{HolidayID}")]
        public async Task<IActionResult> DeleteHoliday(int HolidayID)
        {
            var result=await _HolidayService.DeleteHolidayAsync(HolidayID);
            return Ok(result);
        }

        [HttpGet("GetHolidayByID/{HolidayID}")]
        public async Task<IActionResult> GetHolidayByID(int HolidayID)
        {
          var result=  await _HolidayService.GetHolidayByIDAsync(HolidayID);
          return Ok(result);
        }
    }
    }