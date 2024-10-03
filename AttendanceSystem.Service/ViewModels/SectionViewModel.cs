using AttendanceSystem.PageList;
using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.ViewModels
{
    public class SectionSearchViewModel : BaseOrderSearch
    {
        public string SectionName { get; set; }
        public string DepartmentName { get; set; }
    }
    public class SectionViewModel
    {
        public int CountIndex { get; set; }
        public int SectionID { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
    }
    public class DepartmentAndSectionIdModel
    {
        public int[] DepartmentID { get; set; }
        public int[] SectionID { get; set; }
    }
    public class SectionDesignationIdModel
    {
        public int? DepartmentID { get; set; }
        public int? SectionID { get; set; }
        public int? DesignationID { get; set; }
    }
}
