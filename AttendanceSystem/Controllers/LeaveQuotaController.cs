using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AttendanceSystem.BaseController;
using AttendanceSystem.Services;
using AttendanceSystem.ViewModels;
using System.Collections.Generic;

namespace AttendanceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class LeaveQuotaController : BaseApiController
    {
        protected ILeaveQuotaService _LeaveQuotaService;
        public LeaveQuotaController(ILeaveQuotaService LeaveQuotaService)
        {
            _LeaveQuotaService = LeaveQuotaService;
        }

        [HttpPost("LeaveQuotaList/{EmployeeID}")]
        public async Task<IActionResult> LeaveQuotaList(int EmployeeID)
        {
            var list = await _LeaveQuotaService.LeaveQuotaListAsync(EmployeeID);
            return Ok(list);
        }

        [HttpPost("CreateLeaveQuota")]
        public async Task<IActionResult> CreateLeaveQuota(List<LeaveQuotaViewModel> model)
        {
           var result= await _LeaveQuotaService.InsertIntoLeaveQuotaAsync(model, Convert.ToInt32(CurrentUserDetails.EmployeeID),CurrentUserDetails.FiscalYearID);
            return Ok(result);
        }
    }
    }