using Dapper;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttendanceSystem.Cryptography;
using AttendanceSystem.DapperServices;
using AttendanceSystem.Domains;
using AttendanceSystem.GenericRepository;
using AttendanceSystem.Helpers;
using AttendanceSystem.ViewModels;

namespace AttendanceSystem.Services
{
    public class AccountService : IAccountService
    {
        private readonly IGenericRepository<UserRoles> _userRolesRepository;
        private readonly IDapperRepository _dapperRepository;
        public AccountService(IGenericRepository<UserRoles> userRolesRepository, IDapperRepository dapperRepository)
        {
            _userRolesRepository = userRolesRepository;
            _dapperRepository = dapperRepository;
        }
        public async Task<UserRolesViewModel> AuthenticateAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user =await GetUserAsync(new TokenRequestViewModel { UserName=username});

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!Hash.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();

            var result = new UserRolesViewModel()
            {
                UserRolesID = user.UserRolesID,
                UserName = user.UserName,
                BranchID=user.BranchID,
                Company=user.Company,
                EmployeeID=user.EmployeeID,
                Employee=user.Employee,
                RoleID=user.RoleID,
                Role=user.Role
            };
            return result;
        }

        public async Task<UserRolesViewModel> GetUserAsync(TokenRequestViewModel model)
        {
            try
            {
                var strSQL = new StringBuilder();
                strSQL.AppendFormat(@"SELECT U.[UserRolesID]
                                           ,U.BranchID AS CompanyID
                                          ,U.[EmployeeID]
                                          ,U.[RoleID]
                                          ,U.[UserName]
                                          ,U.[PasswordHash]
                                          ,U.[PasswordSalt]
                                          ,U.[CreatedTS]
                                          ,U.[CreatedBy]
                                          ,U.[ModifiedTS]
                                          ,U.[ModifiedBy]
                                    	  ,R.Name AS Role
                                    	  ,E.FirstName+' '+E.LastName AS Employee
                                    	  ,C.CompanyName AS Company
                                      FROM [dbo].[UserRoles] U
                                      LEFT JOIN CompanyProfile C ON U.BranchID=C.CompanyProfileID
                                      LEFT JOIN Employee E ON U.EmployeeID=E.EmployeeID
                                      LEFT JOIN Roles R ON U.RoleID=R.ID
                                      WHERE 1=1  ");

                if (!string.IsNullOrEmpty(model.UserName))
                {
                    strSQL.AppendFormat(@" AND UserName=@UserName ");
                }
                DynamicParameters _parameters = new DynamicParameters();
                _parameters.Add("@UserName", model.UserName);
                return await _dapperRepository.ExecuteQueryFirstOrDefaultAsync<UserRolesViewModel>(strSQL.ToString(), _parameters);
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
