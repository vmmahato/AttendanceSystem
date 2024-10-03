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

    public class CompanyProfileController : BaseApiController
    {
        protected ICompanyProfileService _companyProfileService;
        private readonly JWTConfig _jwtSettings;

        public CompanyProfileController(ICompanyProfileService companyProfileService, IOptions<JWTConfig> jwtSettings)
        {
            _companyProfileService = companyProfileService;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("CompanyProfileList")]
        public async Task<IActionResult> CompanyProfileList(CompanyProfileSearchViewModel model)
        {
            
            var list = await _companyProfileService.CompanyProfileListAsync(model);
            return Ok(list.ToJson());
        }

        [HttpPost("CreateCompanyProfile")]
        public async Task<IActionResult> CreateCompanyProfile([FromForm] CompanyProfileViewModel model)
        {
            model.CreatedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
            model.CompanyFolderURL = _jwtSettings.CompanyFolderURL;
            var result= await _companyProfileService.InsertIntoCompanyProfileAsync(model);
            return Ok(result);
        }

        [HttpPost("UpdateCompanyProfile")]
        public async Task<IActionResult> UpdateCompanyProfile([FromForm] CompanyProfileViewModel model)
        {
            model.ModifiedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
            model.CompanyFolderURL = _jwtSettings.CompanyFolderURL;
            var result=await _companyProfileService.UpdateCompanyProfileAsync(model);
            return Ok(result);
        }

        [HttpPost("DeleteCompanyProfile/{CompanyProfileID}")]
        public async Task<IActionResult> DeleteCompanyProfile(int CompanyProfileID)
        {
            var result=await _companyProfileService.DeleteCompanyProfileAsync(CompanyProfileID,_jwtSettings.CompanyFolderURL);
            return Ok(result);
        }

        [HttpGet("GetCompanyProfileByID/{CompanyProfileID}")]
        public async Task<IActionResult> GetCompanyProfileByID(int CompanyProfileID)
        {
          var result=  await _companyProfileService.GetCompanyProfileByIDAsync(CompanyProfileID, _jwtSettings.CompanyFolderURL);
            return Ok(result);
        }
        [HttpGet("DDLCompanyList")]
        public async Task<IActionResult> DDLCompanyList()
        {
            var result = await _companyProfileService.DDLCompanyListAsync();
            return Ok(result);
        }
    }
    }