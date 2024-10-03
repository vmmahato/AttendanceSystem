using Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AttendanceSystem.Domains
{
  public  class Kaj:BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int KajID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime FromDate { get; set; }
        public TimeSpan? FromTime { get; set; }
        public DateTime ToDate { get; set; }
        public TimeSpan? ToTime { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
