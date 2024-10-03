using Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AttendanceSystem.Domains
{
    public class Device : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeviceID { get; set; }
        public int DeviceModelID { get; set; }
        public string DeviceName { get; set; }
        public int DeviceNumber { get; set; }
        public string IPAddress { get; set; }
        public int CommunicationPort { get; set; }
        public int SerialPort { get; set; }
        public string CommunicationPassword { get; set; }
        public string CommunicationMode { get; set; }
        public string SerialNumber { get; set; }
        public bool IsActive { get; set; }
        public bool IsRegister { get; set; }
        public string TerminalIP { get; set; }
        public string Branch { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
        public bool IsDelete { get; set; }
    }
}
