using AttendanceSystem.PageList;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.ViewModels
{
    public class DeviceSearchViewModel : BaseOrderSearch
    {
        public string DeviceName { get; set; }
        public string DeviceNumber { get; set; }
        public string DeviceModelID { get; set; }
    }
    public class DeviceViewModel
    {
        public int CountIndex { get; set; }
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
        public bool DeviceStatus { get; set; } = false;
    }

    public class DeviceSearchLogsViewModel : BaseOrderSearch
    {
        public Guid DeviceLogsID { get; set; }
        public int DeviceNumber { get; set; }
    }
    public class DeviceLogsViewModel
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

    public class DeviceSearchModelViewModel : BaseOrderSearch
    {
        public int DeviceModelID { get; set; }
        public string DeviceModelName { get; set; }
    }
    public class DeviceModelViewModel
    {
        public int DeviceModelID { get; set; }
        public string DeviceModelName { get; set; }
        public bool IsRealTime { get; set; }
        public bool IsActive { get; set; }
        public string TerminalIP { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
