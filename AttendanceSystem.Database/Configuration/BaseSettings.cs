using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.Database.Configuration
{
    public class BaseSetting : ISettings
    {
        public string ApplicationType { get; set; }
        public string FileUploadPath { get; set; }
        public string FileServerUrl { get; set; }
        public string ApplicationUrl { get; set; }
    }
}
