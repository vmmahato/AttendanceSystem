﻿using Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AttendanceSystem.Domains
{
   public class WeeklyRoster:BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WeeklyRosterID { get; set; }
        public int FiscalYear { get; set; }
        public int EmployeeID { get; set; }
        public int DayID { get; set; }
        public string DayName { get; set; }
        public int ShiftID { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime ApplicableDate { get; set; }
    }
}