using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.UserClaimModel
{
    public class UserClaimsInformation
    {
        public int UserRolesID { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public int CompanyID { get; set; }
        public int EmployeeID { get; set; }
        public int RoleID { get; set; }
        public string Company { get; set; }
        public string Employee { get; set; }
        public int FiscalYearID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

    }
}
