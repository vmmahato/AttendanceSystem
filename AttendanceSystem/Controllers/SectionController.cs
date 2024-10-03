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
using System.Collections.Generic;

namespace AttendanceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class SectionController : BaseApiController
    {
        protected ISectionService _SectionService;

        public SectionController(ISectionService SectionService)
        {
            _SectionService = SectionService;
        }

        [HttpPost("SectionList")]
        public async Task<IActionResult> SectionList(SectionSearchViewModel model)
        {
            
            var list = await _SectionService.SectionListAsync(model);
            return Ok(list.ToJson());
        }


        [HttpPost("CreateSection")]
        public async Task<IActionResult> CreateSection(SectionViewModel model)
        {
           model.CreatedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
           var result= await _SectionService.InsertIntoSectionAsync(model);
            return Ok(result);
        }

        [HttpPost("UpdateSection")]
        public async Task<IActionResult> UpdateSection(SectionViewModel model)
        {
            model.ModifiedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
            var result=await _SectionService.UpdateSectionAsync(model);
            return Ok(result);
        }

        [HttpPost("DeleteSection/{SectionID}")]
        public async Task<IActionResult> DeleteSection(int SectionID)
        {
            var result=await _SectionService.DeleteSectionAsync(SectionID);
            return Ok(result);
        }

        [HttpGet("GetSectionByID/{SectionID}")]
        public async Task<IActionResult> GetSectionByID(int SectionID)
        {
          var result=  await _SectionService.GetSectionByIDAsync(SectionID);
            return Ok(result);
        }
        [HttpGet("DDLSectionList/{DepartmentID}")]
        public async Task<IActionResult> DDLSectionList(int DepartmentID)
        {
            var result = await _SectionService.DDLSectionListAsync(DepartmentID);
            return Ok(result);
        }
        [HttpPost("DDLDepartmentWiseSectionListAsync")]
        public async Task<IActionResult> DDLDepartmentWiseSectionListAsync(DepartmentAndSectionIdModel model)
        {
            var result = await _SectionService.DDLDepartmentWiseSectionListAsync(model);
            return Ok(result);
        }
    }
    }