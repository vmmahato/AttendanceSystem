using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.ViewModels
{
    public class DashboardAbsentCountWithViewModel
    {
        public int TotalAbsentCount { get; set; }
        public IEnumerable<DashboardAbsentViewModel> List { get; set; }
    }
   public class DashboardAbsentViewModel
    {
        public string Name { get; set; }
        public string Department { get; set; }
        public int EmployeeID { get; set; }
        public bool Absent { get; set; }
    }

    public class DashboardHolidayViewModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    public class DashboardLeaveViewModel
    {
        public string Name { get; set; }
        public string Department { get; set; }
    }

    public class DashboardVisitorViewModel
    {
        public string Visitor { get; set; }
        public string Employee { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
    public class DashboardCounts
    {
        public int DepartmentCount { get; set; }
        public int EmployeesCount { get; set; }
        public int ActiveUserCount { get; set; }
    }
    public class DashboardAttendanceLogViewModel
    {
        public int PuntchCount { get; set; }
        public string EmployeeName { get; set; }
        public int DeviceNumber { get; set; }
        public Nullable<DateTime> Puntchdate { get; set; }
        public string DateStatus { get; set; }
    }
    public class DashboardAttendanceStatusViewModel
    {
        public string EmployeeName { get; set; }
        public string StartDateStatus { get; set; }
    }

    public class DashboardEmployeeStatusViewModel
    {
        public int TotalAbsentCount { get; set; }
        public IEnumerable<DashboardAbsentViewModel> List { get; set; }
    }

    public class EmployeeStatusViewModel
    {
        public string Name { get; set; }
        public string Department { get; set; }
        public int EmployeeID { get; set; }
        public bool LateCount { get; set; }
        public bool AbsentCount { get; set; }
    }

    public class StatusViewModel
    {
        public int LateCount { get; set; }
        public int AbsentCount { get; set; }
        public int OfficeVisitCount { get; set; }
        public int KajCount { get; set; }
        public int LeaveCount { get; set; }
        public int PresentCount { get; set; }
        public IEnumerable<EmployeeStatusViewModel> List { get; set; }
    }
    public class DashboardDeviceStatusViewModel
    {
        public int DeviceNumber { get; set; }
        public string Status { get; set; }
    }

}
