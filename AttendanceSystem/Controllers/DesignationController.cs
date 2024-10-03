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

    public class DesignationController : BaseApiController
    {
        protected IDesignationService _designationService;

        public DesignationController(IDesignationService designationService)
        {
            _designationService = designationService;
        }

        [HttpPost("DesignationList")]
        public async Task<IActionResult> DesignationList(DesignationSearchViewModel model)
        {
            
            var list = await _designationService.DesignationListAsync(model);
            return Ok(list.ToJson());
        }


        [HttpPost("CreateDesignation")]
        public async Task<IActionResult> CreateDesignation(DesignationViewModel model)
        {
           model.CreatedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
           var result= await _designationService.InsertIntoDesignationAsync(model);
            return Ok(result);
        }

        [HttpPost("UpdateDesignation")]
        public async Task<IActionResult> UpdateDesignation(DesignationViewModel model)
        {
            model.ModifiedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
            var result=await _designationService.UpdateDesignationAsync(model);
            return Ok(result);
        }

        [HttpPost("DeleteDesignation/{DesignationID}")]
        public async Task<IActionResult> DeleteDesignation(int DesignationID)
        {
            var result=await _designationService.DeleteDesignationAsync(DesignationID);
            return Ok(result);
        }

        [HttpGet("GetDesignationByID/{DesignationID}")]
        public async Task<IActionResult> GetDesignationByID(int DesignationID)
        {
          var result=  await _designationService.GetDesignationByIDAsync(DesignationID);
            return Ok(result);
        }
        [HttpGet("DDLDesignationList")]
        public async Task<IActionResult> DDLDesignationList()
        {
            var result = await _designationService.DDLDesignationListAsync();
            return Ok(result);
        }
        
    }
    }