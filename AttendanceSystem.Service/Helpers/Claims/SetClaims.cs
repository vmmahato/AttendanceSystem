using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using AttendanceSystem.UserClaimTypesModel;
using AttendanceSystem.ViewModels;

namespace AttendanceSystem.Helpers
{
   public static class SetClaims
    {
        /// <summary>
        /// set up claims after authenticated
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static ClaimsIdentity CreateClaims(UserRolesViewModel User, CurrentFiscalYearDetails FiscalYear)
        {
            var claims = new List<Claim>();
            string Name = string.Empty;
            string Company = string.Empty;
            if (!string.IsNullOrEmpty(User.User))
            {
                Name = User.User;
            }
            if (!string.IsNullOrEmpty(User.Company))
            {
                Company = User.Company;
            }
            if (User != null)
            {
                var userClaims = new Claim[] {
                    new Claim(ClaimTypes.Name, User.UserName),
                    new Claim(ClaimTypes.Role,User.Role),
                    new Claim(UserClaimTypes.Name, Name),
                    new Claim(UserClaimTypes.Company, Company),
                    new Claim(UserClaimTypes.UserRoleID, User.UserRolesID.ToString()),
                    new Claim("LoggedOn", DateTime.Now.ToString()),
                    new Claim(UserClaimTypes.CompanyID, User.BranchID.ToString()),
                    new Claim(UserClaimTypes.EmployeeID, User.EmployeeID.ToString()),
                    new Claim(UserClaimTypes.RoleID, User.RoleID.ToString())
                   };
                claims.AddRange(userClaims);
                if (FiscalYear !=null)
                {
                    var fiscalYearClaims = new Claim[] {
                    new Claim(UserClaimTypes.FiscalYearID, FiscalYear.FiscalYearID.ToString()),
                    new Claim(UserClaimTypes.FromDate, FiscalYear.FromDate.ToString()),
                    new Claim(UserClaimTypes.ToDate, FiscalYear.ToDate.ToString()),
                     new Claim(UserClaimTypes.FromDateString, FiscalYear.FromDate.ToString("yyyy-MM-dd")),
                    new Claim(UserClaimTypes.ToDateString, FiscalYear.ToDate.ToString("yyyy-MM-dd"))
                    };
                    claims.AddRange(fiscalYearClaims);
                }
            }

            return new ClaimsIdentity(claims);
        }
    }
}
