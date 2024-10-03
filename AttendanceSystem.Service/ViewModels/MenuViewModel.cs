using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.ViewModels
{
  public  class MenuViewModel
    {
        public int ID { get; set; }
        public string MenuName { get; set; }
        public string MenuType { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int ModifiedBy { get; set; }
        public bool ProvinceAccess { get; set; }
        public bool DivisionAccess { get; set; }
    }
}
