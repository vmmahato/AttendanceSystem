using AttendanceSystem.Helpers;
using AttendanceSystem.PageList;
using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.ViewModels
{
    public class OfficeVisitViewModel
    {
        public int OfficeVisitID { get; set; }
        public int EmployeeID { get; set; }
        public string VisitorName { get; set; }
        public DateTime FromDate { get; set; }
        public string FromTime { get; set; }
        public DateTime ToDate { get; set; }
        public string ToTime { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
    }
    public class OfficeVisitModel
    {
        public int CountIndex { get; set; }
        public int OfficeVisitID { get; set; }
        public string EmployeeName { get; set; }
        public string VisitorName { get; set; }
        public int EmployeeID { get; set; }
        public string FromDate { get; set; }
        public TimeSpan? FromTime { get; set; }
        public string ToDate { get; set; }
        public TimeSpan? ToTime { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public string FromTimeString
        {
            get
            {
                if (FromTime == null)
                { return string.Empty; }
                else { return SharedServices.ConvertTimeSpanToString(FromTime); }
            }
        }

        public string ToTimeString
        {
            get
            {
                if (ToTime == null)
                { return string.Empty; }
                else { return SharedServices.ConvertTimeSpanToString(ToTime); }
            }
        }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
    }

    public class OfficeVisitSearchViewModel : BaseOrderSearch
    {
        public int EmployeeID { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string VisitorName { get; set; }
    }
}
