using Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AttendanceSystem.Domains
{
  public  class LeaveOpeningBalance:BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LeaveOpeningBalanceID { get; set; }
        public int EmployeeID { get; set; }
        public string Type { get; set; }
        public decimal? Value { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
        public int FiscalYear { get; set; }
    }
}
