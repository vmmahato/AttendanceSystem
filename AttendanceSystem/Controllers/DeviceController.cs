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

    public class DeviceController : BaseApiController
    {
        protected IDeviceService _DeviceService;

        public DeviceController(IDeviceService DeviceService)
        {
            _DeviceService = DeviceService;
        }

        [HttpPost("DeviceList")]
        public async Task<IActionResult> DeviceList(DeviceSearchViewModel model)
        {
            
            var list = await _DeviceService.DeviceListAsync(model);
            return Ok(list.ToJson());
        }


        [HttpPost("CreateDevice")]
        public async Task<IActionResult> CreateDevice(DeviceViewModel model)
        {
           model.CreatedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
           var result= await _DeviceService.InsertIntoDeviceAsync(model);
            return Ok(result);
        }

        [HttpPost("UpdateDevice")]
        public async Task<IActionResult> UpdateDevice(DeviceViewModel model)
        {
            model.ModifiedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
            var result=await _DeviceService.UpdateDeviceAsync(model);
            return Ok(result);
        }

        [HttpPost("DeleteDevice/{DeviceID}")]
        public async Task<IActionResult> DeleteDevice(int DeviceID)
        {
            var result=await _DeviceService.DeleteDeviceAsync(DeviceID);
            return Ok(result);
        }

        [HttpGet("GetDeviceByID/{DeviceID}")]
        public async Task<IActionResult> GetDeviceByID(int DeviceID)
        {
          var result=  await _DeviceService.GetDeviceByIDAsync(DeviceID);
            return Ok(result);
        }

        [HttpGet("GetDeviceDetailsWithStatus")]
        public async Task<IActionResult> GetDeviceDetailsWithStatus()
        {
            var result = await _DeviceService.GetDeviceDetailsWithStatus();
            return Ok(result);
        }
        [HttpGet("DDLDeviceList")]
        public async Task<IActionResult> DDLDeviceList()
        {
            var result = await _DeviceService.DDLDeviceListAsync();
            return Ok(result);
        }
        [HttpGet("DDLDeviceModelList")]
        public async Task<IActionResult> DDLDeviceModelList()
        {
            var result = await _DeviceService.DDLDeviceModelListAsync();
            return Ok(result);
        }
    }
    }