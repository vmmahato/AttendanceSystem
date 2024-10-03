using Core.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AttendanceSystem.ViewModels;

namespace AttendanceSystem.Services
{
  public  interface IMenuService:IService
    {
        Task<IEnumerable<ParentMenuViewModel>> GetMenuListAsync(string Role);
    }
}
