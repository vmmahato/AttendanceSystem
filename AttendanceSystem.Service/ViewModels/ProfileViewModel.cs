using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.ViewModels
{
   public class ProfileViewModel
    {
        public string Logo { get; set; }
        public string TaxClearanceDocument { get; set; }
        public string VatDocument { get; set; }
        public string RegisterDocument { get; set; }
    }

    public class ImageTypesViewModel
    {
        public string Employee { get; set; }
        public string Company { get; set; }
    }
}
