using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AttendanceSystem.BaseController;
using AttendanceSystem.PageExtension;
using AttendanceSystem.Services;
using AttendanceSystem.ViewModels;

namespace AttendanceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class OfficeVisitController : BaseApiController
    {
        protected IOfficeVisitService _officeVisitService;

        public OfficeVisitController(IOfficeVisitService officeVisitService)
        {
            _officeVisitService = officeVisitService;
        }

        [HttpPost("OfficeVisitList")]
        public async Task<IActionResult> OfficeVisitList(OfficeVisitSearchViewModel model)
        {
            var list = await _officeVisitService.OfficeVisitListAsync(model);
            return Ok(list.ToJson());
        }


        [HttpPost("CreateOfficeVisit")]
        public async Task<IActionResult> CreateOfficeVisit(OfficeVisitViewModel model)
        {
           model.CreatedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
           var result= await _officeVisitService.InsertIntoOfficeVisitAsync(model);
            return Ok(result);
        }

        [HttpPost("UpdateOfficeVisit")]
        public async Task<IActionResult> UpdateOfficeVisit(OfficeVisitViewModel model)
        {
            model.ModifiedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
            var result=await _officeVisitService.UpdateOfficeVisitAsync(model);
            return Ok(result);
        }

        [HttpPost("DeleteOfficeVisit/{OfficeVisitID}")]
        public async Task<IActionResult> DeleteOfficeVisit(int OfficeVisitID)
        {
            var result=await _officeVisitService.DeleteOfficeVisitAsync(OfficeVisitID);
            return Ok(result);
        }

        [HttpGet("GetOfficeVisitByID/{OfficeVisitID}")]
        public async Task<IActionResult> GetOfficeVisitByID(int OfficeVisitID)
        {
          var result=  await _officeVisitService.GetOfficeVisitByIDAsync(OfficeVisitID);
            return Ok(result);
        }
        #region Kaj Approve
        [HttpPost("GetPendingOfficeVisitList")]
        public async Task<IActionResult> GetPendingOfficeVisitList(OfficeVisitSearchViewModel model)
        {
            var list = await _officeVisitService.GetPendingOfficeVisitListAsync(model);
            return Ok(list.ToJson());
        }
        [HttpPost("UpdatePendingOfficeVisit")]
        public async Task<IActionResult> UpdatePendingOfficeVisit(OfficeVisitModel model)
        {
            model.ModifiedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
            var result = await _officeVisitService.UpdatePendingOfficeVisitAsync(model);
            return Ok(result);
        }
        #endregion
    }
}