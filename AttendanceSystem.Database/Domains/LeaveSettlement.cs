using Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AttendanceSystem.Domains
{
    public class LeaveSettlement : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SettlementID { get; set; }
        public int EmployeeID { get; set; }
        public int LeaveID { get; set; }
        public decimal? OpeningBalance { get; set; }
        public decimal? LeaveTaken { get; set; }
        public decimal? RemainingLeave { get; set; }
        public decimal? CarryToNext { get; set; }
        public decimal? Pay { get; set; }
        public decimal? SettlingLeave { get; set; }
        public int FiscalYear { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
