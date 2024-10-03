using Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AttendanceSystem.Domains
{
   public class FixedRoster:BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FixedRosterID { get; set; }
        public int FiscalYear { get; set; }
        public int EmployeeID { get; set; }
        public int ShiftID { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime ApplicableDate { get; set; }
    }
}
