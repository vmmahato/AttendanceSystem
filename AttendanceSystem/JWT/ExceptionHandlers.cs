using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceSystem.JWT
{
    public static class ExceptionHandlers
    {
        public static void HandleExceptionTypes(IApplicationBuilder app)
        {
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Use(async (context, next) =>
                {
                    var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;

                    //when authorization has failed, should retrun a json message to client
                    if (error != null && error.Error is SecurityTokenExpiredException)
                    {
                        ErrorResponse.ServerRequestError(context, "Unauthorized", 0001, "Access Token Expired");
                    }
                    //when orther error, retrun a error message json to client
                    else if (error != null && error.Error != null)
                    {
                        ErrorResponse.ServerRequestError(context, "Unauthorized", StatusCodes.Status500InternalServerError, "Internal Server Error");
                    }
                    //when no error, do next.
                    else await next();
                });
            });
        }
    }
}
