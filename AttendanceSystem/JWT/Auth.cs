using AttendanceSystem.AppSettings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.JWT
{
    public static class Auth
    {
        public static void JWTAuthentication(IServiceCollection services,IConfigurationSection JWT)
        {
            services.Configure<JWTConfig>(JWT);
            var JWTSettings = JWT.Get<JWTConfig>();
            var key = Encoding.ASCII.GetBytes(JWTSettings.SecretKey);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(x =>
           {
               x.Events = new JwtBearerEvents
               {
                   OnTokenValidated = context =>
                   {
                       var user = "";// context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                       // JwtSecurityToken RequestToken= context.SecurityToken as JwtSecurityToken; for fetching Token string
                      // var UserID = context.Principal.Claims.FirstOrDefault(x => x.Type == "UserID").Value;
                      // var user = UserService.GetUserByID(Convert.ToInt32(UserID));
                       if (user == null)
                       {
                           ErrorResponse.ServerError(context,"Unauthorized",StatusCodes.Status400BadRequest, "User doesnot exist");
                           //   context.Fail("Unauthorized"); since this returns alwas 401
                       }
                       return Task.CompletedTask;
                   },
                   OnAuthenticationFailed = context =>
                   {
                       if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                           ErrorResponse.TokenError(context,"AccessTokenExpired", StatusCodes.Status400BadRequest, "Access Token Expired");
                       }
                       else
                       {
                           ErrorResponse.TokenError(context, "Unauthorized",StatusCodes.Status400BadRequest, "Invalid Access Token");
                       }
                       return Task.CompletedTask;
                   },
                   OnForbidden = context =>
                   {
                       if (context.Response.StatusCode==StatusCodes.Status403Forbidden)
                       {
                           ErrorResponse.ForbiddenError(context, "Unauthorized", StatusCodes.Status403Forbidden, "You are not authorized");
                       }
                       return Task.CompletedTask;
                   }
               };
               x.RequireHttpsMetadata = false;
            //   x.SaveToken = true;
               x.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(key),
                   ValidateIssuer = false,
                   ValidateAudience = false,
                   LifetimeValidator= CustomLifetimeValidator,
                   ValidateLifetime = true,
                   ClockSkew = TimeSpan.Zero //expire
               };
           });
        }

        private static bool CustomLifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken tokenToValidate, TokenValidationParameters @param)
        {
            if (expires != null)
            {
                return expires > DateTime.UtcNow;
            }
            return false;
        }
    }
}
