using AttendanceSystem.PageList;
using System;
using System.Collections.Generic;

namespace AttendanceSystem.ViewModels
{
    public class ReportSearchViewModel : BaseOrderSearch
    {
        public string ReportType { get; set; }
        public int[] EmployeeID { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Month { get; set; }
        public int FiscalYearID { get; set; }
    }
    public class DailyReportViewModel
    {
        public string Name { get; set; }
        public string Estimated_IN { get; set; }
        public string Estimated_OUT { get; set; }
        public double WorkHour { get; set; }
        public string Actual_IN { get; set; }
        public string Actual_OUT { get; set; }
        public string BreakOut { get; set; }
        public string BreakIn { get; set; }
        public string TeaOut { get; set; }
        public string TeaIn { get; set; }
        public double workedhour { get; set; }
        public double OT { get; set; }
        public double EarlyIN { get; set; }
        public double EarlyOUT { get; set; }
        public double LateIN { get; set; }
        public double LateOUT { get; set; }
        public string Remarks { get; set; }
    }
    public class MonthlyReportViewModel
    {
        public string Department { get; set; }
        public string Name { get; set; }
        public List<MonthlyReport> ReportList { get; set; }

    }
    public class MonthlyReport
    {
        public string Date { get; set; }
        public string Day { get; set; }
        public string Estimated_IN { get; set; }
        public string Estimated_OUT { get; set; }
        public double WorkHour { get; set; }
        public string Actual_IN { get; set; }
        public string Actual_OUT { get; set; }
        public string BreakOut { get; set; }
        public string BreakIn { get; set; }
        public string TeaOut { get; set; }
        public string TeaIn { get; set; }
        public double workedhour { get; set; }
        public double OT { get; set; }
        public double EarlyIN { get; set; }
        public double EarlyOUT { get; set; }
        public double LateIN { get; set; }
        public double LateOUT { get; set; }
        public string Remarks { get; set; }
    }
    public class MonthlyReportList
    {
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public int DepartmentID { get; set; }
        public string Department { get; set; }
        public string Date { get; set; }
        public string Day { get; set; }
        public string Estimated_IN { get; set; }
        public string Estimated_OUT { get; set; }
        public double WorkHour { get; set; }
        public string Actual_IN { get; set; }
        public string Actual_OUT { get; set; }
        public string BreakOut { get; set; }
        public string BreakIn { get; set; }
        public string TeaOut { get; set; }
        public string TeaIn { get; set; }
        public double workedhour { get; set; }
        public double OT { get; set; }
        public double EarlyIN { get; set; }
        public double EarlyOUT { get; set; }
        public double LateIN { get; set; }
        public double LateOUT { get; set; }
        public string Remarks { get; set; }
    }
    public class RosterReportViewModel
    {
        public string Name { get; set; }
        public List<RosterModel> ShiftList { get; set; }
    }
    public class RosterReportModel
    {
        public List<RosterReportViewModel> EmployeeShiftList { get; set; }
        public List<HeaderModel> Days { get; set; }

    }

    public class HeaderModel
    {
        public int Day { get; set; }
        public string DayName { get; set; }
    }
    public class RosterModel
    {
        public string Name { get; set; }
        public int FiscalYear { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string DayName { get; set; }
        public int ShiftID { get; set; }
        public string ShiftName { get; set; }
        public int EmployeeID { get; set; }
    }

    public class DailyLeaveReportViewModel
    {
        public string Name { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string LeaveDay { get; set; }
        public string Status { get; set; }
        public string LeaveName { get; set; }
        public string ReplacementLeaveType { get; set; }
    }

    public class DailyManualReportViewModel
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public int EnrollID { get; set; }
        public int DeviceNumber { get; set; }
        public string PuntchTime { get; set; }

    }

    public class DailyOfficeVisitReportViewModel
    {
        public string Name { get; set; }
        public string FromDate { get; set; }
        public string FromTime { get; set; }
        public string ToDate { get; set; }
        public string ToTime { get; set; }
        public string Status { get; set; }
        public string VisitorName { get; set; }

    }

    public class MonthlyManualReportViewModel
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public int EnrollID { get; set; }
        public int DeviceNumber { get; set; }
        public string AttendanceDate { get; set; }
        public string PuntchTime { get; set; }

    }
    public class MissingpunchReportViewModel
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string PunchDate { get; set; }
        public string SearchDate { get; set; }
        public string StatusPuntch { get; set; }

    }



}
