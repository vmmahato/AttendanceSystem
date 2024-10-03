using Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;
using System.Data.SqlClient;
using AttendanceSystem.Database;
using EFCore.BulkExtensions;

namespace AttendanceSystem.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly AttendanceSystemDbContext _context;
        private DbSet<T> _entities;
        public GenericRepository(AttendanceSystemDbContext context)
        
        {
            this._context = context;
            
            _entities = this._context.Set<T>();
        }

        #region Methods
        /// <summary>
        /// entity that you know already exists in the database but which is not currently being tracked by the context then you can tell the context to track the entity using this method
        /// </summary>
        /// <returns>
        /// True if the attachment is done, else false (possible duplicate)
        /// </returns>
        public virtual bool Attach(T entity, Func<T, bool> predicate)
        {
            var entityExists = this.Entities.Local.Any(predicate);
            if (entityExists == false)
            {
                this.Entities.Attach(entity);
            }
            return !entityExists;
        }
        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Insert(T entity)
        {
            //ref https://www.michaelgmccarthy.com/2016/08/24/entity-framework-addorupdate-is-a-destructive-operation/
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var entry = this._context.Entry(entity);
            if (entry.State == EntityState.Modified)
            {
                _context.Entry(entity).State = EntityState.Modified;
            }
            else
            {
                this.Entities.Add(entity);
            }
        }

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Insert(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            foreach (var entity in entities)
                this.Entities.Add(entity);

        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            _context.ChangeTracker.AutoDetectChangesEnabled = true;
            //this.Entities.Update(entity);
            _context.Entry(entity).State = EntityState.Modified;

        }

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Update(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            this.Entities.Remove(entity);

        }

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Delete(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            foreach (var entity in entities)
                this.Entities.Remove(entity);

        }

        #endregion

        public int SaveChanges()
        {
            //foreach(var entry in this._context.ChangeTracker.Entries())
            //{
            //    var entity = entry.Entity;
            //    if (entry.State == EntityState.Deleted)
            //    {
            //        entry.State = EntityState.Modified;
            //        entity.GetType().GetProperty("IsDelete").SetValue(entity, true);
            //    }
            //}
            return this._context.SaveChanges();
        }
        public Task<int> SaveChangesAsync()
        {
            //   foreach(var entry in this._context.ChangeTracker.Entries())
            //{
            //    var entity = entry.Entity;
            //    if (entry.State == EntityState.Deleted)
            //    {
            //        entry.State = EntityState.Modified;
            //        entity.GetType().GetProperty("IsDelete").SetValue(entity, true);
            //    }
            //}
            return this._context.SaveChangesAsync();
        }
        #region Properties

        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<T> Table
        {
            get
            {
                return this.Entities;
            }
        }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        public virtual IQueryable<T> TableNoTracking
        {
            get
            {
                return this.Entities.AsNoTracking();
            }
        }
        [Obsolete("Just for test. do not use.")]
        public virtual object FirstOrDefault()
        {
            return this.Entities.AsNoTracking().FirstOrDefault();
        }

        public async Task  BulkInsertAsync(List<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
         await  this._context.BulkInsertAsync(entities);
        }

        public async Task BulkUpdateAsync(List<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));
          await  this._context.BulkUpdateAsync(entities);
        }

        /// <summary>
        /// Entities
        /// </summary>
        protected virtual DbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _context.Set<T>();
                return _entities;
            }
        }

        #endregion
    }
}
