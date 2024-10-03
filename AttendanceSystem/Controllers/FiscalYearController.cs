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

    public class FiscalYearController : BaseApiController
    {
        protected IFiscalYearService _fiscalYearService;

        public FiscalYearController(IFiscalYearService fiscalYearService)
        {
            _fiscalYearService = fiscalYearService;
        }

        [HttpPost("FiscalYearList")]
        public async Task<IActionResult> FiscalYearList(FiscalYearSearchViewModel model)
        {
            
            var list = await _fiscalYearService.FiscalYearListAsync(model);
            return Ok(list.ToJson());
        }


        [HttpPost("CreateFiscalYear")]
        public async Task<IActionResult> CreateFiscalYear(FiscalYearViewModel model)
        {
           model.CreatedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
           var result= await _fiscalYearService.InsertIntoFiscalYearAsync(model);
            return Ok(result);
        }

        [HttpPost("UpdateFiscalYear")]
        public async Task<IActionResult> UpdateFiscalYear(FiscalYearViewModel model)
        {
            model.ModifiedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
            var result=await _fiscalYearService.UpdateFiscalYearAsync(model);
            return Ok(result);
        }

        [HttpPost("DeleteFiscalYear/{FiscalID}")]
        public async Task<IActionResult> DeleteFiscalYear(int FiscalID)
        {
            var result=await _fiscalYearService.DeleteFiscalYearAsync(FiscalID);
            return Ok(result);
        }

        [HttpGet("GetFiscalYearByID/{FiscalID}")]
        public async Task<IActionResult> GetFiscalYearByID(int FiscalID)
        {
          var result=  await _fiscalYearService.GetFiscalYearByIDAsync(FiscalID);
            return Ok(result);
        }
        [HttpGet("DDLFiscalYearList")]
        public async Task<IActionResult> DDLFiscalYearList()
        {
            var result = await _fiscalYearService.DDLFiscalYearListAsync();
            return Ok(result);
        }
    }
    }