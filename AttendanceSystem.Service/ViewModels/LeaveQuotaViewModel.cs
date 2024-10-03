using AttendanceSystem.PageList;
using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.ViewModels
{
    public class LeaveQuotaSearchViewModel : BaseOrderSearch
    {
        public int? EmployeeID { get; set; }
    }
    public class LeaveQuotaViewModel
    {
        public int CountIndex { get; set; }
        public int LeaveQuotaID { get; set; }
        public int LeaveID { get; set; }
        public int DesignationID { get; set; }
        public string LeaveType { get; set; }
        public decimal LeaveBalance { get; set; }
        public string LeaveIncrementPeroid { get; set; }
        public string ApplicableGender { get; set; }
        public bool IsPaidLeave { get; set; }
        public bool IsLeaveCarryable { get; set; }
        public bool IsReplacementLeave { get; set; }
        public int FiscalYear { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
