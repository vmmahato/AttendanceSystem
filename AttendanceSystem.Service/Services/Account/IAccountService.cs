using Core.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AttendanceSystem.Domains;
using AttendanceSystem.ViewModels;

namespace AttendanceSystem.Services
{
  public  interface IAccountService: IService
    {
        Task<UserRolesViewModel> AuthenticateAsync(string username, string password);
        Task<UserRolesViewModel> GetUserAsync(TokenRequestViewModel model);
    }
}
