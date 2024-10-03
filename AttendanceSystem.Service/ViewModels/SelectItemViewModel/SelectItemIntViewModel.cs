using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.ViewModels
{
   public class SelectItemIntViewModel
    {
        public int ID { get; set; }
        public string Value { get; set; }
    }
    public class SelectItemsIntViewModel
    {
        public int DepartmentID { get; set; }
        public int ID { get; set; }
        public string Value { get; set; }
    }

    public class SelectItemViewModel
    {
        public string ID { get; set; }
        public string Value { get; set; }
    }

    public class SelectTextValueModel
    {
        public string Text { get; set; }
        public int Value { get; set; }
    }
    public class SelectItemValueViewModel
    {
        public string ID { get; set; }
        public string Value { get; set; }
        public decimal? Amount { get; set; }
    }
}
