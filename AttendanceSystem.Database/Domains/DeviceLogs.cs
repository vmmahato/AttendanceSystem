using Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.Domains
{
    public class DeviceLogs:BaseEntity
    {
        public Guid DeviceLogsID { get; set; }
        public int DeviceNumber { get; set; }
        public long EnrollID { get; set; }
        public DateTime PunchDate { get; set; }
        public string VerificationMode { get; set; }
        public string InOutMode { get; set; }
        public bool IsProcessed { get; set; }
        public DateTime FetchedDate { get; set; }
        public string DeviceModel { get; set; }
        public string TerminalIP { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
