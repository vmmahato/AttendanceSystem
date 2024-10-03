using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.ViewModels
{
   public class RolesViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool ManagerAccess { get; set; }
        public bool UserAccess { get; set; }
    }
}
