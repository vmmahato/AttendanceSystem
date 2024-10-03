using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AttendanceSystem.Domains;
using AttendanceSystem.GenericRepository;
using AttendanceSystem.UserClaimTypesModel;
using AttendanceSystem.ViewModels;
using AttendanceSystem.DapperServices;

namespace AttendanceSystem.Services
{
    public class MenuService : IMenuService
    {
        private readonly IGenericRepository<Menu> _menuRepository;
        private IDapperRepository _dapperRepository;
        public MenuService(IGenericRepository<Menu> menuRepository, IDapperRepository dapperRepository)
        {
            _menuRepository = menuRepository;
            _dapperRepository = dapperRepository;
        }
        /// <summary>
        /// Admin By Default
        /// </summary>
        /// <param name="Role"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ParentMenuViewModel>> GetMenuListAsync(string Role)
        {
            var result = new List<ParentMenuViewModel>();
            var strSQL = new StringBuilder();
            strSQL.AppendFormat(@"SELECT 
                                  [ID]
                                 ,[MenuName]
                                 ,[MenuType]
                                 ,[ParentID]
                                 ,[Icon]
                                 ,[Link]
                                 ,[ManagerAccess]
                                 ,[SuperAdminAccess]
                                 ,[AdminAccess]
                                 ,[SupervisorAccess]
                             FROM [dbo].[Menu] WHERE 1=1");
            #region Filter
            if (Role == "Manager")
            {
                strSQL.AppendFormat(" AND ManagerAccess<>0");
            }
            if (Role == "User")
            {
                strSQL.AppendFormat(" AND UserAccess<>0");
            }
            #endregion

            var MenuList = await _dapperRepository.ExecuteQueryAsync<ChildMenuBasedOnRoles>(strSQL.ToString(), null);


            var ParentMenuList = MenuList.Where(x => x.ParentID == 0).AsQueryable();
           
            if (MenuList.Count() == 0) return null;
            if (Role == UserClaimTypes.SuperAdmin)
            {
                foreach (var parent in ParentMenuList)
                {
                    var ParentWithChild = new ParentMenuViewModel()
                    {
                        ParentMenu = new ParentMenuNameWithIcon()
                        {
                            Icon = parent.Icon,
                            MenuName = parent.MenuName,
                            Link=parent.Link
                        },
                        MenuList = MenuList.Where(x => x.ParentID == parent.ID).ToList()
                    };
                    result.Add(ParentWithChild);
                }
                return result;
            }
            else if (Role ==UserClaimTypes.Manager)
            {
                foreach (var parent in ParentMenuList.Where(x=>x.ManagerAccess==true))
                {
                    var ParentWithChild = new ParentMenuViewModel()
                    {
                        ParentMenu = new ParentMenuNameWithIcon()
                        {
                            Icon = parent.Icon,
                            MenuName = parent.MenuName,
                            Link = parent.Link
                        },
                        MenuList = MenuList.Where(x => x.ParentID == parent.ID && x.ManagerAccess==true).ToList()
                    };
                    result.Add(ParentWithChild);
                }
                return result;

            }
            else if(Role == UserClaimTypes.Admin)
            {
                foreach (var parent in ParentMenuList.Where(x => x.AdminAccess == true))
                {
                    var ParentWithChild = new ParentMenuViewModel()
                    {
                        ParentMenu = new ParentMenuNameWithIcon()
                        {
                            Icon = parent.Icon,
                            MenuName = parent.MenuName,
                            Link = parent.Link
                        },
                        MenuList = MenuList.Where(x => x.ParentID == parent.ID && x.AdminAccess == true).ToList()
                    };
                    result.Add(ParentWithChild);
                }
                return result;
            }
            else if (Role == UserClaimTypes.Supervisor)
            {
                foreach (var parent in ParentMenuList.Where(x => x.SupervisorAccess == true))
                {
                    var ParentWithChild = new ParentMenuViewModel()
                    {
                        ParentMenu = new ParentMenuNameWithIcon()
                        {
                            Icon = parent.Icon,
                            MenuName = parent.MenuName,
                            Link = parent.Link
                        },
                        MenuList = MenuList.Where(x => x.ParentID == parent.ID && x.SupervisorAccess == true).ToList()
                    };
                    result.Add(ParentWithChild);
                }
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
