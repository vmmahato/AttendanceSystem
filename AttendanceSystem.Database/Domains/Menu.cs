using Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AttendanceSystem.Domains
{
    public class Menu:BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string MenuName { get; set; }
        public string MenuType { get; set; }
        public DateTime CreatedTS { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedTS { get; set; }
        public int ModifiedBy { get; set; }
        public string Icon { get; set; }
        public string Link { get; set; }
        public bool ManagerAccess { get; set; }
        public bool UserAccess { get; set; }
        public int DisplayOrder { get; set; }
    }
}
