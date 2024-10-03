using AttendanceSystem.PageList;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.ViewModels
{
    public class HolidaySearchViewModel : BaseOrderSearch
    {
        public string HolidayName { get; set; }
        public string HolidayType { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
    public class HolidayViewModel
    {
        public int CountIndex { get; set; }
        public int HolidayID { get; set; }
        public string HolidayName { get; set; }
        public string HolidayType { get; set; }
        public string ApplicableReligion { get; set; }
        public string ApplicableGender { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int FiscalYear { get; set; }
        public string Description { get; set; }
        public bool IsDepartmentWiseHoliday { get; set; }
        public int[] DepartmentID { get; set; }
        public int[] SectionID { get; set; }
        public string DepartmentIDString { get; set; }
        public string SectionIDString { get; set; }
        public string WeekendDay { get; set; }
        public bool IsWeekendLeave { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
    }
    public class HolidayModel
    {
        public int CountIndex { get; set; }
        public int HolidayID { get; set; }
        public string HolidayName { get; set; }
        public string HolidayType { get; set; }
        public string ApplicableReligion { get; set; }
        public string ApplicableGender { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int FiscalYear { get; set; }
        public string Description { get; set; }
        public bool IsDepartmentWiseHoliday { get; set; }
        public int[] DepartmentID { get; set; }
        public int[] SectionID { get; set; }
        public string DepartmentIDString { get; set; }
        public string SectionIDString { get; set; }
        public string WeekendDay { get; set; }
        public bool IsWeekendLeave { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
