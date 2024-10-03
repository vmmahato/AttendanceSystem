using Dapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttendanceSystem.DapperServices;
using AttendanceSystem.Domains;
using AttendanceSystem.GenericRepository;
using AttendanceSystem.UserClaimTypesModel;
using AttendanceSystem.ViewModels;
using AttendanceSystem.PageExtension;
using AttendanceSystem.Cryptography;

namespace AttendanceSystem.Services
{
    public class UserRolesService : IUserRolesService
    {
        private IDapperRepository _dapperRepository;
        private IGenericRepository<Roles> _rolesRepository;
        private IGenericRepository<UserRoles> _userRolesRepository;
        public UserRolesService(IDapperRepository dapperRepository,
                                IGenericRepository<Roles> rolesRepository,
                                IGenericRepository<UserRoles> userRolesRepository)
        {
            _dapperRepository = dapperRepository;
            _rolesRepository = rolesRepository;
            _userRolesRepository = userRolesRepository;
        }

        public async Task<IEnumerable<SelectItemIntViewModel>> DDLRolesAsync(string Role)
        {
            var result = new List<SelectItemIntViewModel>();
            var query = _rolesRepository.TableNoTracking.AsQueryable();
            if (Role == UserClaimTypes.Manager)
            {
                query = query.Where(x => x.ManagerAccess == true).AsQueryable();
            }
            if (Role == UserClaimTypes.Admin)
            {
                query = query.Where(x => x.AdminAccess == true).AsQueryable();
            }
            if (Role == UserClaimTypes.SuperAdmin)
            {
                query = query.AsQueryable();
            }
            if (Role == UserClaimTypes.Supervisor)
            {
                query = query.Where(x => x.SupervisorAccess == true).AsQueryable();
            }
            return await query.Select(x => new SelectItemIntViewModel()
                {
                    ID = x.ID,
                    Value = x.Name
                }).ToListAsync();
        }

        public async Task<AccountResult> DeleteUserRolesAsync(int UserRolesID)
        {
            var result = new AccountResult();
            var ExistedUserRoles =  GetUserRolesByID(UserRolesID);
            if (ExistedUserRoles != null)
            {
                _userRolesRepository.Delete(ExistedUserRoles);
                await _userRolesRepository.SaveChangesAsync();
            }
            return result;
        }

        public async Task<IPagedList<UserRolesViewModel>> GetRolesListAsync(SearchUserRolesViewModel model)
        {
            var strSQL = new StringBuilder();
            strSQL.AppendFormat(@"SELECT UR.[UserRolesID]
                                          ,UR.[BranchID]
                                          ,UR.[EmployeeID]
                                          ,UR.[RoleID]
                                          ,UR.[UserName]
                                          ,UR.[CreatedTS]
                                          ,UR.[CreatedBy]
                                          ,UR.[ModifiedTS]
                                          ,UR.[ModifiedBy]
                                    	  ,R.Name AS Role
                                    	  ,E.FirstName+' '+E.LastName AS Employee
                                    	  ,C.BranchName 
                                      FROM [dbo].[UserRoles] UR
                                      LEFT JOIN CompanyProfile C ON UR.BranchID=C.CompanyProfileID
                                      LEFT JOIN Employee E ON UR.EmployeeID=E.EmployeeID
                                      LEFT JOIN Roles R ON UR.RoleID=R.ID
                                      WHERE 1=1 ");
            DynamicParameters _parameters = new DynamicParameters();
            return await _dapperRepository.ExecuteQueryWithPagedListAsync<UserRolesViewModel>(strSQL.ToString(), _parameters, model.PageSize, model.PageNo, model.OrderBy ?? "[EmployeeID]");
        }

        public UserRoles GetUserRolesByID(int UserRolesID)
        {
            return _userRolesRepository.Table.FirstOrDefault(x => x.UserRolesID == UserRolesID);
        }

        public async Task<UserRolesViewModel> GetUserRolesByIDAsync(int UserRolesID)
        {
            var strSQL = new StringBuilder();
            strSQL.AppendFormat(@"SELECT UR.[UserRolesID]
                                          ,UR.[BranchID]
                                          ,UR.[EmployeeID]
                                          ,UR.[RoleID]
                                          ,UR.[UserName]
                                          ,UR.[CreatedTS]
                                          ,UR.[CreatedBy]
                                          ,UR.[ModifiedTS]
                                          ,UR.[ModifiedBy]
                                    	  ,R.Name AS Role
                                    	  ,E.FirstName+' '+E.LastName AS Employee
                                    	  ,C.BranchName 
                                      FROM [dbo].[UserRoles] UR
                                      LEFT JOIN CompanyProfile C ON UR.BranchID=C.CompanyProfileID
                                      LEFT JOIN Employee E ON UR.EmployeeID=E.EmployeeID
                                      LEFT JOIN Roles R ON UR.RoleID=R.ID
                                      WHERE UR.UserRolesID=@UserRolesID ");
            DynamicParameters _parameters = new DynamicParameters();
            _parameters.Add("@UserRolesID", UserRolesID);
            return await _dapperRepository.ExecuteQueryFirstOrDefaultAsync<UserRolesViewModel>(strSQL.ToString(), _parameters);
        }

        public async Task<AccountResult> InsertIntoUserRolesAsync(UserRolesViewModel model)
        {
            var result = new AccountResult();
            if (_userRolesRepository.TableNoTracking.Any(x => x.BranchID == model.BranchID && x.UserName == model.UserName && x.EmployeeID==model.EmployeeID))
            {
                result.Errors = new List<string> { "UserName: " + model.UserName + " is already taken" };
                return result;
            }
                byte[] passwordHash, passwordSalt;
                Hash.CreatePasswordHash(model.Password, out passwordHash, out passwordSalt);
            var data = new UserRoles()
            {
                BranchID = model.BranchID,
                EmployeeID = model.EmployeeID,
                RoleID = model.RoleID,
                UserName = model.UserName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                CreatedBy = model.CreatedBy,
                CreatedTS = DateTime.UtcNow
            };
            _userRolesRepository.Insert(data);
            await _userRolesRepository.SaveChangesAsync();
            return result;
        }

        public async Task<AccountResult> UpdateUserRolesAsync(UserRolesViewModel model)
        {
            var result = new AccountResult();
            var ExistedUserRoles = GetUserRolesByID(model.UserRolesID);
            if (ExistedUserRoles.UserName != model.UserName)
            {
                if (_userRolesRepository.TableNoTracking.Any(x => x.BranchID == model.BranchID && x.UserName == model.UserName && x.EmployeeID == model.EmployeeID))
                {
                    result.Errors = new List<string> { "UserName: " + model.UserName + " is already taken" };
                    return result;
                }

            }

            if (ExistedUserRoles != null)
            {
                ExistedUserRoles.BranchID = model.BranchID;
                ExistedUserRoles.EmployeeID = model.EmployeeID;
                ExistedUserRoles.RoleID = model.RoleID;
                ExistedUserRoles.UserName = model.UserName;
                if (!string.IsNullOrEmpty(model.Password))
                {
                    byte[] passwordHash, passwordSalt;
                    Hash.CreatePasswordHash(model.Password, out passwordHash, out passwordSalt);
                    ExistedUserRoles.PasswordHash = passwordHash;
                    ExistedUserRoles.PasswordSalt = passwordSalt;
                }
                 
                ExistedUserRoles.ModifiedBy = model.ModifiedBy;
                ExistedUserRoles.ModifiedTS = DateTime.UtcNow;
                await _userRolesRepository.SaveChangesAsync();
            }
            return result;
        }
    }
}
