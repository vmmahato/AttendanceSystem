using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.PageList
{
    public abstract class BasePageSearch { 
        public int PageNo { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }
}
