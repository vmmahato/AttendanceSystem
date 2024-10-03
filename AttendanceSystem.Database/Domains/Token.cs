using Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AttendanceSystem.Domains
{
   public class Token:BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TokenID { get; set; }
        public int UserID { get; set; }
        public string ClientID { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiryTime { get; set; }
        public DateTime CreatedTS { get; set; }

    }
}
