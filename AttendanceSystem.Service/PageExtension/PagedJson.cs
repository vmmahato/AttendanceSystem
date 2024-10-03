using System;
using System.Collections.Generic;
using System.Text;
using AttendanceSystem.PageExtension;

namespace AttendanceSystem.PageExtension
{
    public class PagedJson<T>
    {
        public IPagedList<T> Data { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }

        public bool HasPreviousPage
        {
            get { return (PageNo > 1); }
        }
        public bool HasNextPage
        {
            get { return (PageNo < TotalPages); }
        }
    }

}
