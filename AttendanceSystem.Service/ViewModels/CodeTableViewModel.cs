using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.ViewModels
{
  public class CodeTableViewModel
    {
        public int ID { get; set; }
        public string TableName { get; set; }
        public string Description { get; set; }
        public int? DisplayOrder { get; set; }
        public string Value { get; set; }
        public DateTime? CreatedTS { get; set; }
    }
}
