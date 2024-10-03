using Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AttendanceSystem.Domains
{
    public class FiscalYears : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        public bool IsDelete { get; set; }
    }
}
