using Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AttendanceSystem.Domains
{
   public class WorkShift:BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShiftID { get; set; }
        public string ShiftCode { get; set; }
        public string ShiftName { get; set; }
        public TimeSpan? ShiftStart { get; set; }
        public TimeSpan? ShiftEnd { get; set; }
        public TimeSpan? BeginingIn { get; set; }
        public TimeSpan? EndingIn { get; set; }
        public TimeSpan? BeginingOut { get; set; }
        public TimeSpan? EndingOut { get; set; }
        public TimeSpan? LateIn { get; set; }
        public string LeaveEarly { get; set; }
        public bool IsMustCheckIn { get; set; }
        public bool IsMustCheckOut { get; set; }
        public bool IsLateIn { get; set; }
        public bool IsEarlyLeave { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
        public bool IsDelete { get; set; }
    }
}
