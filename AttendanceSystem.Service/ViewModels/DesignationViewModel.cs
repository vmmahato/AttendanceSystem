using AttendanceSystem.PageList;
using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.ViewModels
{
    public class DesignationSearchViewModel : BaseOrderSearch
    {
        public string DesignationName { get; set; }
        public string DesignationLevel { get; set; }
    }
    public class DesignationViewModel
    {
        public int CountIndex { get; set; }
        public int DesignationID { get; set; }
        public string DesignationName { get; set; }
        public string DesignationLevel { get; set; }
        public decimal? Salary { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
