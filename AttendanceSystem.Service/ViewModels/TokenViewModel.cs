using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.ViewModels
{
    public class TokenRequestModel
    {
        public int UserRolesID { get; set; }
        public string UserName { get; set; }
        public string User { get; set; }
        public string Role { get; set; }
        public DateTime ExpirationToken { get; set; } 
        public string RefreshToken { get; set; } 
        public IEnumerable<ParentMenuViewModel> MenuList { get; set; }
        public string DashBoard { get; set; }
        public string Setting { get; set; }
    }

    public class ParentMenuViewModel
    {
      public ParentMenuNameWithIcon ParentMenu { get; set; }
      public IEnumerable<ChildMenuBasedOnRoles> MenuList { get; set; }
    }
    public class ParentMenuNameWithIcon
    {
        public string MenuName { get; set; }
        public string Icon { get; set; }
        public string Link { get; set; }
    }
    public class ChildMenuBasedOnRoles
    {
        public int ID { get; set; }
        public string MenuName { get; set; }
        public string MenuType { get; set; }
        public string Icon { get; set; }
        public string Link { get; set; }
        public bool ManagerAccess { get; set; }
        public bool AdminAccess { get; set; }
        public bool SupervisorAccess { get; set; }
        public bool SuperAdminAccess { get; set; }
        public int ParentID { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class TokenModels
    {
        public string token { get; set; } // jwt token
        public DateTime expiration { get; set; } // expiry time
        public string refresh_token { get; set; } // refresh token
    }

    public class TokenResponse
    {
        public TokenRequestModel TokenDetails { get; set; }
        public string Token { get; set; }
    }

    public class CurrentFiscalYearDetails
    {
        public int FiscalYearID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }


}
