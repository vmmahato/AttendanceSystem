using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AttendanceSystem.PageExtension
{
    public static class PagedListExtension
    {
        /// <summary>
        /// Get Paged List Asynchronously
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount">if provided, it assumes paging has been done already so this is just for wrapping purpose.</param>
        /// <returns></returns>
        public static async Task<IPagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, int pageNo, int pageSize, int? totalCount = null)
        {
            var pagedList = new PagedList<T>();
            await pagedList.CreateAsync(source, pageNo, pageSize, totalCount);
            return pagedList;
        }
        /// <summary>
        /// Get Paged List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount">if provided, it assumes paging has been done already so this is just for wrapping purpose.</param>
        /// <returns></returns>
        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> source, int pageNo, int pageSize, int? totalCount = null)
        {
            var pagedList = new PagedList<T>();
            pagedList.Create(source, pageNo, pageSize, totalCount);
            return pagedList;
        }
        /// <summary>
        /// Get Paged List
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount">if provided, it assumes paging has been done already so this is just for wrapping purpose.</param>
        /// <returns></returns>
        public static IPagedList<T> ToPagedList<T>(this IQueryable<T> source, int pageNo, int pageSize, int? totalCount = null)
        {
            if (totalCount != null)
            {
                return new PagedList<T>(source, pageNo, pageSize, totalCount.Value);
            }
            else
            {
                return new PagedList<T>(source, pageNo, pageSize);
            }
        }
    }
}
