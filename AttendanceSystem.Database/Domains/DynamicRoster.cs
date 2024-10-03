using Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AttendanceSystem.Domains
{
    public class DynamicRoster:BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DynamicRosterID { get; set; }
        public int FiscalYear { get; set; }
        public DateTime ActualDate { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string DayName { get; set; }
        public int EmployeeID { get; set; }
        public int? ShiftID { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime ApplicableDate { get; set; }
        public int? ReplacementEmployeeID { get; set; }
        public string Type { get; set; }
        public int? ReplacementShiftID { get; set; }
    }
}
