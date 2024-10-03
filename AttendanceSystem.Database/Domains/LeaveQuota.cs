using Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AttendanceSystem.Domains
{
    public class LeaveQuota : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LeaveQuotaID { get; set; }
        public int LeaveID { get; set; }
        public int DesignationID { get; set; }
        public string LeaveType { get; set; }
        public decimal LeaveBalance { get; set; }
        public string LeaveIncrementPeroid { get; set; }
        public string ApplicableGender { get; set; }
        public bool IsPaidLeave { get; set; }
        public bool IsLeaveCarryable { get; set; }
        public bool IsReplacementLeave { get; set; }
        public int FiscalYear { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
