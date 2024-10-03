using AttendanceSystem.PageList;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.ViewModels
{
    public class DefaultLeaveSearchViewModel : BaseOrderSearch
    {
        public string LeaveCode { get; set; }
        public string LeaveName { get; set; }
    }
    public class DefaultLeaveViewModel
    {
        public int CountIndex { get; set; }
        public int LeaveID { get; set; }
        public string LeaveCode { get; set; }
        public string LeaveName { get; set; }
        public int LeaveDays { get; set; }
        public string LeaveIncrement { get; set; }
        public string ApplicableGender { get; set; }
        public string Description { get; set; }
        public bool IsReplacementLeave { get; set; }
        public bool IsPaidLeave { get; set; }
        public bool IsLeaveCarryable { get; set; }
        public int FiscalYear { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
