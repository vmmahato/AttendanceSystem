using Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.GenericRepository
{
   public interface IGenericRepository<T> where T : BaseEntity
    {
            //T GetById(object id);
            /// <summary>
            /// entity that you know already exists in the database but which is not currently being tracked by the context then you can tell the context to track the entity using this method
            /// </summary>
            /// <returns>
            /// True if the attachment is done, else false (possible duplicate)
            /// </returns>
            bool Attach(T entity, Func<T, bool> predicate);
            /// <summary>
            /// Insert with check for Primary Key already existing. If Primary key already exists in database then it will do update.
            /// </summary>
            /// <param name="entity"></param>        
            void Insert(T entity);
            //inserts a list of object
            void Insert(IEnumerable<T> entities);
            /// <summary>
            /// Update entity.
            /// Should be tracked entity
            /// </summary>
            /// <param name="entity"></param>
            void Update(T entity);
            /// <summary>
            /// Update entites
            /// Should be tracked entities
            /// </summary>
            /// <param name="entites"></param>
            void Update(IEnumerable<T> entites);
            /// <summary>
            /// Delete entity
            /// </summary>
            /// <param name="entity"></param>
            void Delete(T entity);
            /// <summary>
            /// Delete Entities
            /// </summary>
            /// <param name="entites"></param>
            void Delete(IEnumerable<T> entites);
            /// <summary>
            /// Save Changes to Database
            /// </summary>
            /// <returns>return no of rows affected</returns>
            int SaveChanges();
            /// <summary>
            /// Save changes to Database Asynchronosly
            /// </summary>
            /// <returns>return no of rows affected</returns>
            Task<int> SaveChangesAsync();
            /// <summary>
            /// Changes in table are tracked.
            /// 
            /// Result:
            /// Higher memory usage than without tracking
            /// </summary>
            IQueryable<T> Table { get; }
            /// <summary>
            /// Changes in table won't be tracked.
            /// Uses less memory
            /// Result:
            /// Insert, Update, Delete won't work (probably, haven't checked)
            /// </summary>
            IQueryable<T> TableNoTracking { get; }
            /// <summary>
            /// EF Core Bulk Extension
            /// </summary>
            Task BulkInsertAsync(List<T> entities);
            /// <summary>
            /// Bulk Update
            /// </summary>
            Task BulkUpdateAsync(List<T> entities);
            
        }
    }
