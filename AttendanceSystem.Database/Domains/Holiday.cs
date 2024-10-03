using Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AttendanceSystem.Domains
{
    public class Holiday : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HolidayID { get; set; }
        public string HolidayName { get; set; }
        public string HolidayType { get; set; }
        public string ApplicableReligion { get;set; }
        public string ApplicableGender { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int FiscalYear { get; set; }
        public string Description { get; set; }
        public bool IsDepartmentWiseHoliday { get; set; }
        public string DepartmentID { get; set; }
        public string SectionID { get; set; }
        public string WeekendDay { get; set; }
        public bool IsWeekendLeave { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
