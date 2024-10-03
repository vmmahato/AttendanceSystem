using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AttendanceSystem.BaseController;
using AttendanceSystem.PageExtension;
using AttendanceSystem.Services;
using AttendanceSystem.UserClaimTypesModel;
using AttendanceSystem.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserRolesController : BaseApiController
    {
        protected IUserRolesService _userRolesService;
        public UserRolesController(IUserRolesService userRolesService)
        {
            _userRolesService = userRolesService;
        }
        [HttpPost("UserRolesListAsync")]
        public async Task<IActionResult> UserRolesListAsync(SearchUserRolesViewModel model)
        {

            var list = await _userRolesService.GetRolesListAsync(model);
            return Ok(list.ToJson());
        }


        [HttpPost("CreateUserRolesAsync")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUserRolesAsync(UserRolesViewModel model)
        {
            model.CreatedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
            var result = await _userRolesService.InsertIntoUserRolesAsync(model);
            return Ok(result);
        }

        [HttpPost("UpdateUserRolesAsync")]
        public async Task<IActionResult> UpdateUserRolesAsync(UserRolesViewModel model)
        {
            model.ModifiedBy = Convert.ToInt32(CurrentUserDetails.EmployeeID);
            var result = await _userRolesService.UpdateUserRolesAsync(model);
            return Ok(result);
        }

        [HttpPost("DeleteUserRolesAsync/{UserRoleID}")]
        public async Task<IActionResult> DeleteUserRolesAsync(int UserRoleID)
        {
            var result = await _userRolesService.DeleteUserRolesAsync(UserRoleID);
            return Ok(result);
        }

        [HttpGet("GetUserRolesByIDAsync")]
        public async Task<IActionResult> GetUserRolesByIDAsync(int UserRoleID)
        {
            var result = await _userRolesService.GetUserRolesByIDAsync(UserRoleID);
            return Ok(result);
        }

        [HttpGet("DDLRolesAsync")]
        public async Task<IActionResult> DDLRolesAsync()
        {
            var result = await _userRolesService.DDLRolesAsync(CurrentUserDetails.Role);
            return Ok(result);
        }
    }
}