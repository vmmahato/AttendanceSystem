using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.AppSettings
{
 public class JWTConfig
    {
        public string SecretKey { get; set; }
        public string ExpireTime { get; set; }
        public string ClientId { get; set; }
        public string FolderURL { get; set; }
        public string CompanyFolderURL { get; set; }
        public string EmployeeFolderURL { get; set; }
    }
}
