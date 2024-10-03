using System.Collections.Generic;

namespace AttendanceSystem.PageExtension
{
    public interface IPagedList<T> : IList<T>
    {
        int PageNo { get; }
        int PageSize { get; }
        int TotalCount { get; }
        int TotalPages { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }
    }
}


