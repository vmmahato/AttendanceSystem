using AttendanceSystem.PageList;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.ViewModels
{
    public class ManualPuntchSearchViewModel : BaseOrderSearch
    {
        public int? DeviceNumber { get; set; }
        public int? EnrollID { get; set; }
        public DateTime? PunchDate { get; set; }
    
    }
    public class ManualPuntchViewModelResult
    {
        public Guid DeviceLogsID { get; set; }
        public int DeviceNumber { get; set; }
        public int EmployeeId { get; set; }
        public int EnrollID { get; set; }
        public DateTime PunchDate { get; set; }
        public bool IsProcessed { get; set; }
        public DateTime FetchedDate { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
        public string Dateonly { get; set; }
        public string Timeonly { get; set; }
        public string Name { get; set; }
    }
    public class ManualPuntchViewModel
    {
        public Guid DeviceLogsID { get; set; }
        public int DeviceNumber { get; set; }
        public int EmployeeId { get; set; }
        public int EnrollID { get; set; }
        public DateTime PunchDate { get; set; }
        public bool IsProcessed { get; set; }
        public DateTime FetchedDate { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
        public string Dateonly { get; set; }
        public string Timeonly { get; set; }
        public string DepartmentID { get; set; }
        public string SectionID { get; set; }
        public string Name { get; set; }
    }

    public class ManualPuntchModel
    {
        public Guid DeviceLogsID { get; set; }
        public int DeviceNumber { get; set; }
        public int EmployeeId { get; set; }
        public int EnrollID { get; set; }
        public DateTime PunchDate { get; set; }
        public bool IsProcessed { get; set; }
        public DateTime FetchedDate { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public string Dateonly { get; set; }
        public string Timeonly { get; set; }
        public int[] DepartmentID { get; set; }
        public int[] SectionID { get; set; }
        public string Name { get; set; }
    }


}
