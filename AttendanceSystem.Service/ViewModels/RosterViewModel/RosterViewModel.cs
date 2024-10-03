using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.ViewModels
{

    public class FiscalYearWithFixedRosterViewModel
    {
        public int FiscalYear { get; set; }
        public int CreatedBy { get; set; }
        public List<EmployeeShiftViewModel> EmployeeShiftList { get; set; }
    }

    public class EmployeeShiftViewModel
    {
        public int EmployeeID { get; set; }
        public DateTime ApplicableDate { get; set; }
        public List<RosterViewModel> ShiftList { get; set; }
    }
   public class RosterViewModel
    {
        public string Name { get; set; }
        public int ShiftID { get; set; }
        public int FiscalYear { get; set; }
        public int EmployeeID { get; set; }
        public DateTime ApplicableDate { get; set; }
        public string ApplicableDateString { get; set; }
        public IEnumerable<SelectItemIntViewModel> ShiftList { get; set; }
    }

    public class WeeklyRosterViewModel
    {
        public string DayName { get; set; }
        public int DayID { get; set; }
        public string Name { get; set; }
        public int EmployeeID { get; set; }
        public int ShiftID { get; set; }
        public int FiscalYear { get; set; }
        public DateTime ApplicableDate { get; set; }
        public string ApplicableDateString { get; set; }
        public IEnumerable<SelectItemIntViewModel> ShiftList { get; set; }
    }

    public class UpdateGroupedFixedRosterViewModel
    {
        public int CreatedBy { get; set; }
        public int FiscalYear { get; set; }
        public List<GroupedFixedRosterViewModel> EmployeeList { get; set; }
    }
    public class GroupedFixedRosterViewModel
    {
        public string Name { get; set; }
        public int EmployeeID { get; set; }
        public DateTime ApplicableDate { get; set; }
        public string ApplicableDateString { get; set; }
        public IEnumerable<RosterViewModel> ShiftList { get; set; }
    }

    public class WeekWithEmployeeShiftList
    {
        public List<GroupedWeeklyRosterViewModel> EmployeeShiftList { get; set; }
        public List<string> WeekDays { get; set; }
    }
    public class GroupedWeeklyRosterViewModel
    {
        public string Name { get; set; }
        public int EmployeeID { get; set; }
        public string ApplicableDateString { get; set; }
        public DateTime ApplicableDate { get; set; }
        public int FiscalYear { get; set; }
        public List<WeeklyRosterViewModel> EmployeeShiftList { get; set; }
    }

    public class UpdateGroupedWeeklyRosterViewModel
    {
        public int CreatedBy { get; set; }
        public int FiscalYear { get; set; }
        public List<GroupedWeeklyRosterViewModel> EmployeeList { get; set; }
    }

    public class GroupedDynamicRosterViewModel
    {
        public string Name { get; set; }
        public int EmployeeID { get; set; }
        public string ApplicableDateString { get; set; }
        public DateTime ApplicableDate { get; set; }
        public int FiscalYear { get; set; }
        public int Month { get; set; }
        public bool IsOldMonth { get; set; }
        public List<DynamicRosterViewModel> EmployeeShiftList { get; set; }
    }
    public class DynamicRosterViewModel
    {
        public string Name { get; set; }
        public int FiscalYear { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string DayName { get; set; }
        public int EmployeeID { get; set; }
        public int ShiftID { get; set; }
        public DateTime ApplicableDate { get; set; }
        public string ApplicableDateString { get; set; }
        public IEnumerable<SelectItemIntViewModel> ShiftList { get; set; }
        public bool IsOldDate { get; set; } = false;
    }

    public class WeekDaysViewModel
    {
        public int DayID { get; set; }
        public string Day { get; set; }
    }

    public class FiscalWithEmployeeListViewModel
    {
        public int FiscalYearID { get; set; }
        public List<int> EmployeeList { get; set; }
    }
    public class MessageViewModel
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class FiscalWithEmployeeDynamicListViewModel
    {
        public bool IsNepali { get; set; } = true;
        public DateTime NepaliFromDate { get; set; }
        public DateTime NepaliToDate { get; set; }
        public DateTime EnglishFromDate { get; set; }
        public DateTime EnglishToDate { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int FiscalYearID { get; set; }
        public List<int> EmployeeList { get; set; }
    }

    public class DaysInMonthWithEmployeeShiftList
    {
        public List<GroupedDynamicRosterViewModel> EmployeeShiftList { get; set; }
        public IEnumerable<int> Days { get; set; }
        public bool IsOldMonth { get; set; } = false;
        public int Month { get; set; }
        public int CreatedBy { get; set; }
        public int FiscalYear { get; set; }
        public DateTime? NepaliFromDate { get; set; }
        public DateTime? NepaliToDate { get; set; }
        public DateTime? EnglishFromDate { get; set; }
        public DateTime? EnglishToDate { get; set; }
    }

    public class EmployeeWithApplicableDate
    {
        public int EmployeeID { get; set; }
        public string ApplicableDateString { get; set; }
    }
    public class EmployeeHolidayViewModel
    {
        public string ApplicableGender { get; set; }
        public string ApplicableReligion { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int FiscalYear { get; set; }
        public string WeekendDay { get; set; }
        public bool IsWeekendLeave { get; set; }
    }

    public class MonthDay
    {
        public int Month { get; set; }
        public int Day { get; set; }
    }

}
