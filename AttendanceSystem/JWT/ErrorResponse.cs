using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceSystem.JWT
{
    /// <summary>
    /// Error with codes
    /// </summary>
    public static class ErrorResponse
    {
        public static void TokenError(AuthenticationFailedContext context, string State,int StatusCode, string Message)
        {
            context.Response.StatusCode = StatusCode;
            context.Response.ContentType = "application/json";
            context.Response.Headers.Add("Access-Token-Expired", "true");
            context.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                State = State,
                Message = Message
            }));
        }

        public static void ServerError(TokenValidatedContext context, string State,int StatusCode, string Message)
        {
            context.Response.StatusCode = StatusCode;
            context.Response.ContentType = "application/json";
            context.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                State = State,
                Message = Message
            }));
        }

        public static void ServerRequestError(HttpContext context, string State,int StatusCode, string Message)
        {
            context.Response.StatusCode = StatusCode;
            context.Response.ContentType = "application/json";
            context.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                State = State,
                Message = Message
            }));
        }

        public static void ForbiddenError(ForbiddenContext context, string State, int StatusCode, string Message)
        {
            context.Response.StatusCode = StatusCode;
            context.Response.ContentType = "application/json";
            context.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                State = State,
                Message = Message
            }));
        }
    }
}
