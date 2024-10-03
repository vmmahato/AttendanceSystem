using AttendanceSystem.PageList;
using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.ViewModels
{
    public class FiscalYearSearchViewModel : BaseOrderSearch
    {
        public string FiscalYear { get; set; }
        public DateTime? StartYear { get; set; }
        public DateTime? EndYear { get; set; }
    }
    public class FiscalYearViewModel
    {
        public int CountIndex { get; set; }
        public int FiscalID { get; set; }
        public string FiscalYear { get; set; }
        public DateTime StartYear { get; set; }
        public DateTime EndYear { get; set; }
        public DateTime StartDateBS { get; set; }
        public DateTime EndDateBS { get; set; }
        public bool IsCurrentFiscalYear { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
    }
    public class FiscalYearModel
    {
        public int CountIndex { get; set; }
        public int FiscalID { get; set; }
        public string FiscalYear { get; set; }
        public string StartYear { get; set; }
        public string EndYear { get; set; }
        public string StartDateBS { get; set; }
        public string EndDateBS { get; set; }
        public bool IsCurrentFiscalYear { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
