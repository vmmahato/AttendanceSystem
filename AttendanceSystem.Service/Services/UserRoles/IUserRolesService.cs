using Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttendanceSystem.Domains;
using AttendanceSystem.ViewModels;
using AttendanceSystem.PageExtension;

namespace AttendanceSystem.Services
{
   public interface IUserRolesService:IService
    {
        Task<IPagedList<UserRolesViewModel>> GetRolesListAsync(SearchUserRolesViewModel model);
        Task<AccountResult> InsertIntoUserRolesAsync(UserRolesViewModel model);
        UserRoles GetUserRolesByID(int UserRolesID);
        Task<AccountResult> UpdateUserRolesAsync(UserRolesViewModel model);
        Task<AccountResult> DeleteUserRolesAsync(int UserRolesID);
        Task<UserRolesViewModel> GetUserRolesByIDAsync(int UserRolesID);
        Task<IEnumerable<SelectItemIntViewModel>> DDLRolesAsync(string Role);
    }
}
