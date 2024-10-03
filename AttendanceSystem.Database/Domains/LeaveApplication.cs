using Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AttendanceSystem.Domains
{
    public class LeaveApplication : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LeaveApplicationID { get; set; }
        public int EmployeeID { get; set; }
        public string Code { get; set; }
        public int DesignationID { get; set; }
        public int DepartmentID { get; set; }
        public int SectionID { get; set; }
        public int LeaveID { get; set; }
        public string LeaveDay { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Days { get; set; }
        public int ApprovedBy { get; set; }
        public int RemainingLeave { get; set; }
        public int ReplacementEmployeeID { get; set; }
        public bool IsOTLeave { get; set; }
        public bool IsReplacementLeave { get; set; }
        public string ReplacementLeaveType { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public int FiscalYear { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
    }
    public class RemainingLeaveViewModel
    {
        public int RemainingLeave { get; set; }
    }
    public class LeaveAppicantViewModel
    {
        public int EmployeeID { get; set; }
        public int LeaveID { get; set; }
        public int FiscalYear { get; set; }
        public DateTime? CurrentStartDate { get; set; }
    }
}
