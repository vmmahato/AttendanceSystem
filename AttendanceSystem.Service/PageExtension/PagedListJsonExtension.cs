using System;
using System.Collections.Generic;
using System.Text;

namespace AttendanceSystem.PageExtension
{
  public static  class PagedListJsonExtension
    {
        public static PagedJson<T> ToJson<T>(this IPagedList<T> source)
        {
            var pagedData = new PagedJson<T>()
            {
                Data = source,
                PageNo = source.PageNo,
                PageSize = source.PageSize,
                TotalCount = source.TotalCount,
                TotalPages = source.TotalPages
            };
            return pagedData;
        }

    }
}
