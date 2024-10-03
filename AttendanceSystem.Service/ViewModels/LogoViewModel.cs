using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.ViewModels
{
    public class LogoViewModel
    {
        public int ProfileID { get; set; }
        public int ModuleID { get; set; }
        public IFormFile Logo { get; set; }
        public int ModifiedBy {get;set;}
    }
}
