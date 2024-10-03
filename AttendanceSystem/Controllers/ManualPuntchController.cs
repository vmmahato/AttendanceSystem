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

    public class ManualPuntchController : BaseApiController
    {
        protected IManualPuntchService _ManualPuntchService;

        public ManualPuntchController(IManualPuntchService ManualPuntchService)
        {
            _ManualPuntchService = ManualPuntchService;
        }

        [HttpPost("ManualPuntchList")]
        public async Task<IActionResult> ManualPuntchList(ManualPuntchSearchViewModel model)
        {

            var list = await _ManualPuntchService.ManualPuntchListAsync(model);
            return Ok(list.ToJson());
        }


        [HttpPost("CreateManualPuntch")]
        public async Task<IActionResult> CreateManualPuntch(ManualPuntchViewModelResult model)
        {
            model.CreatedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
            var result = await _ManualPuntchService.InsertIntoManualPuntchAsync(model);
            return Ok(result);
        }

        [HttpPost("UpdateManualPuntch")]
        public async Task<IActionResult> UpdateManualPuntch(ManualPuntchViewModelResult model)
        {
            model.ModifiedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
            var result = await _ManualPuntchService.UpdateManualPuntchAsync(model);
            return Ok(result);
        }

        [HttpPost("DeleteManualPuntch/{DeviceLogsID}")]
        public async Task<IActionResult> DeleteManualPuntch(Guid DeviceLogsID)
        {
            var result = await _ManualPuntchService.DeleteManualPuntchAsync(DeviceLogsID);
            return Ok(result);
        }

        [HttpGet("GetManualPuntchByDevicelogsID/{DevicelogsID}")]
        public async Task<IActionResult> GetManualPuntchByDevicelogsID(Guid DevicelogsID)
        {
            var result = await _ManualPuntchService.GetManualPuntchByDevicelogsIDAsync(DevicelogsID);
            return Ok(result);

        }
    }
 }