using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.ViewModels
{
    public class DocumentsViewModel
    {
        public int ProfileID { get; set; }
        public int ModuleID { get; set; }
        public IFormFile RegisterDocument { get; set; }
        public IFormFile VatDocument { get; set; }
        public IFormFile TaxClearanceDocument { get; set; }
        public int ModifiedBy { get; set; }
    }
}
