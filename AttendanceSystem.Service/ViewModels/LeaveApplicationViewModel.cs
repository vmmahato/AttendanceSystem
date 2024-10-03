using AttendanceSystem.PageList;
using Microsoft.AspNetCore.Http;
using System;

namespace AttendanceSystem.ViewModels
{
    public class LeaveApplicationSearchViewModel : BaseOrderSearch
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    public class LeaveApplicationViewModel
    {
        public int CountIndex { get; set; }
        public int LeaveApplicationID { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string ReplacementEmployeeName { get; set; }
        public string ApprovedByEmployee { get; set; }
        public string Code { get; set; }
        public int DesignationID { get; set; }
        public string DesignationName { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName{ get; set; }
        public int SectionID { get; set; }
        public string SectionName { get; set; }
        public int? LeaveID { get; set; }
        public string LeaveType { get; set; }
        public string LeaveDay { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string FromDateString { get; set; }
        public string ToDateString { get; set; }
        public int Days { get; set; }
        public int ApprovedBy { get; set; }
        public string ApprovedByName { get; set; }
        public int? RemainingLeave { get; set; }
        public int ReplacementEmployeeID { get; set; }
        public bool IsOTLeave { get; set; }
        public bool IsReplacementLeave { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public int FiscalYear { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
        public string ReplacementLeaveType { get; set; }
    }


    public class ReplacementEmployeeViewModel
    {
        public int EmployeeID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int ShiftID { get; set; }
        public int FiscalYearID { get; set; }
        public int DesignationID { get; set; }
    }

    public class EmployeeWithShiftIDViewModel
    {
        public string EmployeeName { get; set; }
        public int EmployeeID { get; set; }
        public int EmployeeShiftID { get; set; }
    }

    public class GroupEmployeeWithShiftByEmployeeIDViewModel
    {
        public int EmployeeID { get; set; }
        public int Count { get; set; }
        public EmployeeWithShiftIDViewModel ShiftDetails { get; set; }
    }
}
