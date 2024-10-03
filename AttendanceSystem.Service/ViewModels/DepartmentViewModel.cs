using AttendanceSystem.PageList;
using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.ViewModels
{
    public class DepartmentSearchViewModel : BaseOrderSearch
    {
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
    }
    public class DepartmentViewModel
    {
        public int CountIndex { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
