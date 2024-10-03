using AttendanceSystem.PageList;
using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.ViewModels
{

    public class SearchUserRolesViewModel:BaseOrderSearch
    {
        public int BranchID { get; set; }
    }
        public class UserRolesViewModel
    {
        public int UserRolesID { get; set; }
        public int BranchID { get; set; }
        public string BranchName { get; set; }
        public int EmployeeID { get; set; }
        public string User { get; set; }
        public int RoleID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
        public string Company { get; set; }
        public string Employee { get; set; }
        public string Role { get; set; }
    }
}
