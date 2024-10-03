using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using AttendanceSystem.UserClaimModel;
using AttendanceSystem.UserClaimTypesModel;
using AttendanceSystem.ViewModels;

namespace AttendanceSystem.Helpers
{
    public static class UserHelper
    {
        /// <summary>
        /// Get Current User Information
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static UserClaimsInformation ClaimsInformation(this ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated == false) return null;
            var userClaimsInformation = new UserClaimsInformation();
            foreach (var item in user.Claims)
            {
                if (item.Type == UserClaimTypes.UserRoleID)
                {
                    userClaimsInformation.UserRolesID = Convert.ToInt32(item.Value);
                }
                else if (item.Type == UserClaimTypes.Name)
                {
                    userClaimsInformation.Name = item.Value;
                }
                else if (item.Type == ClaimTypes.Role)
                {
                    userClaimsInformation.Role = item.Value;
                }
                else if (item.Type == UserClaimTypes.CompanyID)
                {
                    userClaimsInformation.CompanyID = Convert.ToInt32(item.Value);
                }
                else if (item.Type == UserClaimTypes.EmployeeID)
                {
                    userClaimsInformation.EmployeeID = Convert.ToInt32(item.Value);
                }
                else if (item.Type == UserClaimTypes.RoleID)
                {
                    userClaimsInformation.RoleID = Convert.ToInt32(item.Value);
                }
                else if (item.Type == UserClaimTypes.Company)
                {
                    userClaimsInformation.Company = item.Value;
                }
                else if (item.Type == UserClaimTypes.Employee)
                {
                    userClaimsInformation.Employee = item.Value;
                }
                else if (item.Type == UserClaimTypes.FiscalYearID)
                {
                    userClaimsInformation.FiscalYearID = Convert.ToInt32(item.Value);
                }

            }
            return userClaimsInformation;
        }
       
    }
}
