using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Cors;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AttendanceSystem.BaseController;
using AttendanceSystem.ViewModels;
using AttendanceSystem.UserClaimTypesModel;
using AttendanceSystem.Services;

namespace AttendanceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountController : BaseApiController
    {
        protected readonly IAccountService _accountService;
        protected readonly IMenuService _menuService;
        protected readonly ITokenService _tokenService;
        protected readonly ICommonService _commonService;

        public AccountController(IAccountService accountService,
                                 IMenuService menuService,
                                 ITokenService tokenService,
                                 ICommonService commonService)
        {
            _accountService = accountService;
            _menuService = menuService;
            _tokenService = tokenService;
            _commonService = commonService;
        }


        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Authenticate([FromBody]LoginViewModel model)
        {
              var user = await _accountService.AuthenticateAsync(model.Username, model.Password);
            if (user == null)
                return BadRequest("Incorrect Username or password.");

            var result = new TokenRequestModel()
            {
                UserRolesID = user.UserRolesID,
                UserName = user.UserName,
                User = user.Employee,
                Role = user.Role
            };
            result.DashBoard = "/Dashboard";
            result.Setting = "/Setting";
            var refreshToken = "";
                
             //   _tokenService.GenerateRefreshToken();
            var tokenString = await _tokenService.GenerateAccessTokenAsync(user, refreshToken);

            result.RefreshToken = tokenString.refresh_token;
            result.ExpirationToken = tokenString.expiration;
            result.MenuList =await _menuService.GetMenuListAsync(user.Role);
            return Ok(new
            {
                Data = result,
                Token = tokenString.token
            });
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await _tokenService.DeleteAllTokenByUserIDAsync(CurrentUserDetails.EmployeeID);
            return Ok(result);
        }

        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> Auth([FromBody] TokenRequestViewModel model) // granttype = "refresh_token"
        {
            // We will return Generic 500 HTTP Server Status Error
            // If we receive an invalid payload
            if (model == null)
            {
                return new StatusCodeResult(500);
            }
            var token = await _tokenService.RefreshTokenAsync(model);
            if (token.TokenExpired) return BadRequest("Token Expired");
            return Ok(token);
        }
    }
}