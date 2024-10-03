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

    public class DepartmentController : BaseApiController
    {
        protected IDepartmentService _DepartmentService;

        public DepartmentController(IDepartmentService DepartmentService)
        {
            _DepartmentService = DepartmentService;
        }

        [HttpPost("DepartmentList")]
        public async Task<IActionResult> DepartmentList(DepartmentSearchViewModel model)
        {
            
            var list = await _DepartmentService.DepartmentListAsync(model);
            return Ok(list.ToJson());
        }


        [HttpPost("CreateDepartment")]
        public async Task<IActionResult> CreateDepartment(DepartmentViewModel model)
        {
           model.CreatedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
           var result= await _DepartmentService.InsertIntoDepartmentAsync(model);
            return Ok(result);
        }

        [HttpPost("UpdateDepartment")]
        public async Task<IActionResult> UpdateDepartment(DepartmentViewModel model)
        {
            model.ModifiedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
            var result=await _DepartmentService.UpdateDepartmentAsync(model);
            return Ok(result);
        }

        [HttpPost("DeleteDepartment/{DepartmentID}")]
        public async Task<IActionResult> DeleteDepartment(int DepartmentID)
        {
            var result=await _DepartmentService.DeleteDepartmentAsync(DepartmentID);
            return Ok(result);
        }

        [HttpGet("GetDepartmentByID/{DepartmentID}")]
        public async Task<IActionResult> GetDepartmentByID(int DepartmentID)
        {
          var result=  await _DepartmentService.GetDepartmentByIDAsync(DepartmentID);
            return Ok(result);
        }
        [HttpGet("DDLDepartmentList")]
        public async Task<IActionResult> DDLDepartmentList()
        {
            var result = await _DepartmentService.DDLDepartmentListAsync();
            return Ok(result);
        }
        
    }
    }