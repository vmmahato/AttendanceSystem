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

    public class KajController : BaseApiController
    {
        protected IKajService _kajService;

        public KajController(IKajService kajService)
        {
            _kajService = kajService;
        }

        [HttpPost("KajList")]
        public async Task<IActionResult> KajList(KajSearchViewModel model)
        {
            
            var list = await _kajService.KajListAsync(model);
            return Ok(list.ToJson());
        }


        [HttpPost("CreateKaj")]
        public async Task<IActionResult> CreateKaj(KajViewModel model)
        {
           model.CreatedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
           var result= await _kajService.InsertIntoKajAsync(model);
            return Ok(result);
        }

        [HttpPost("UpdateKaj")]
        public async Task<IActionResult> UpdateKaj(KajViewModel model)
        {
            model.ModifiedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
            var result=await _kajService.UpdateKajAsync(model);
            return Ok(result);
        }

        [HttpPost("DeleteKaj/{KajID}")]
        public async Task<IActionResult> DeleteKaj(int KajID)
        {
            var result=await _kajService.DeleteKajAsync(KajID);
            return Ok(result);
        }

        [HttpGet("GetKajByID/{KajID}")]
        public async Task<IActionResult> GetKajByID(int KajID)
        {
          var result=  await _kajService.GetKajByIDAsync(KajID);
            return Ok(result);
        }
        #region Kaj Approve
        [HttpPost("GetPendingKajList")]
        public async Task<IActionResult> GetPendingKajListAsync(KajSearchViewModel model)
        {
            var list = await _kajService.GetPendingKajListAsync(model);
            return Ok(list.ToJson());
        }
        [HttpPost("UpdatePendingKajAsync")]
        public async Task<IActionResult> UpdatePendingKajAsync(KajViewModel model)
        {
            model.ModifiedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
            var result = await _kajService.UpdatePendingKajAsync(model);
            return Ok(result);
        }
        #endregion

    }
}