using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace AttendanceSystem.PageExtension
{
    /// <summary>
    /// Paged list
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    [Serializable]
    public class PagedList<T> : List<T>, IPagedList<T>
    {
        /// <summary>
        /// Blank Constructor for Async Paging
        /// </summary>
        public PagedList()
        {

        }
        /// <summary>
        /// Ctor (paging in performed inside)
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        public PagedList(IQueryable<T> source, int pageNo, int pageSize)
        {
            Init(source, pageNo, pageSize);
        }

        /// <summary>
        /// Ctor (paging in performed inside)
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="totalCount">if provided, it assumes paging has been done already so this is just for wrapping purpose.</param>
        public PagedList(IQueryable<T> source, int pageNo, int pageSize, int? totalCount=null)
        {
            Init(source, pageNo, pageSize, totalCount);
        }
        /// <summary>
        /// Async Paging on IQueryable
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount">if provided, it assumes paging has been done already so this is just for wrapping purpose.</param>
        public async Task CreateAsync(IQueryable<T> source, int pageNo, int pageSize, int? totalCount = null)
        {
            await InitAsync(source, pageNo, pageSize, totalCount);
        }

        /// <summary>
        /// Paging on IEnumerable
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount">if provided, it assumes paging has been done already so this is just for wrapping purpose.</param>
        public void Create(IEnumerable<T> source, int pageNo, int pageSize, int? totalCount = null)
        {
            Init(source, pageNo, pageSize, totalCount);
        }
        
        private void Init(IQueryable<T> source, int pageNo, int pageSize, int? totalCount = null)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (pageSize <= 0)
                throw new ArgumentException("pageSize must be greater than zero");

            TotalCount = totalCount ?? source.Count();
            TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                TotalPages++;

            PageSize = pageSize;
            PageNo = pageNo;
            source = totalCount == null ? source.Skip((pageNo - 1) * pageSize).Take(pageSize) : source;

            AddRange(source);
        }
            
        private async Task InitAsync(IQueryable<T> source, int pageNo, int pageSize, int? totalCount = null)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (pageSize <= 0)
                throw new ArgumentException("pageSize must be greater than zero");

            TotalCount = totalCount ?? await source.CountAsync();
            TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                TotalPages++;

            PageSize = pageSize;
            PageNo = pageNo;
            source = totalCount == null ? source.Skip((pageNo - 1) * pageSize).Take(pageSize) : source;

            AddRange(await source.ToListAsync());
        }

        private void Init(IEnumerable<T> source, int pageNo, int pageSize, int? totalCount = null)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (pageSize <= 0)
                throw new ArgumentException("pageSize must be greater than zero");

            TotalCount = totalCount ?? source.Count();
            TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                TotalPages++;

            PageSize = pageSize;
            PageNo = pageNo;
            source = totalCount == null ? source.Skip((pageNo - 1) * pageSize).Take(pageSize) : source;

            AddRange(source.ToList());
        }

        public int PageNo { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }

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
