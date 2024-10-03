using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.PageList
{
    public abstract class BaseOrderSearch : BasePageSearch
    {
        string _orderby = null;
        public string OrderBy
        {
            get
            {
                if (!string.IsNullOrEmpty(_orderby))
                {
                    return _orderby + " " + OrderType;
                }
                return _orderby;
            }
            set { _orderby = value; }
        }
        public string OrderType { get; set; }
    }
}
