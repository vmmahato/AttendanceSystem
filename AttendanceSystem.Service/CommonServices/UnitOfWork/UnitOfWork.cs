using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem.Services
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {

        #region " Properties "
        private readonly DbContext _context;
        private IDbTransaction _transaction;
        private readonly bool _usesDbContext;
        /// <summary>
        /// For _useDbContext=false
        /// If DbContext is being used then it has its own Connection
        /// </summary>
        private IDbConnection _connection;
        #endregion

        public IDbTransaction Transaction { get { return _transaction; } }
        public IDbConnection Connection { get { return _connection; } }

        /// <summary>
        /// For SQL COnnection
        /// </summary>
        /// <param name="connection"></param>
        public UnitOfWork(IDbConnection connection)
        {
            _connection = connection;
            _usesDbContext = false;
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
                _transaction = _connection.BeginTransaction();
            }
        }


        public void SaveChanges()
        {
            if (_usesDbContext)
                _context.SaveChanges();
        }

        public Task SaveChangesAsync()
        {
            if (_usesDbContext)
                return _context.SaveChangesAsync();
            return Task.FromResult<bool>(true);
        }

        public void Commit()
        {
            commit();
        }

        public void Rollback()
        {
            if (_usesDbContext)
            {
                rollback();
            }
            else
            {
                rollbackSql();
            }
        }
        /// <summary>
        /// Called when <see cref="IUnitOfWork"/> is created without `using` block, for manual disposing
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #region " Private Methods "

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // disposing transaction rollbacks the transaction by default if no commit exists.
                    _transaction.Dispose();
                    closeSqlConnection();
                }
            }
            this.disposed = true;
        }
        private void closeSqlConnection()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        private void commit()
        {
            _transaction.Commit();
        }

        private void rollback()
        {
            _transaction.Rollback();
            // http://blog.oneunicorn.com/2011/04/03/rejecting-changes-to-entities-in-ef-4-1/

            foreach (var entry in _context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        // Note - problem with deleted entities:
                        // When an entity is deleted its relationships to other entities are severed. 
                        // This includes setting FKs to null for nullable FKs or marking the FKs as conceptually null (don’t ask!) 
                        // if the FK property is not nullable. You’ll need to reset the FK property values to 
                        // the values that they had previously in order to re-form the relationships. 
                        // This may include FK properties in other entities for relationships where the 
                        // deleted entity is the principal of the relationship–e.g. has the PK 
                        // rather than the FK. I know this is a pain–it would be great if it could be made easier in the future, but for now it is what it is.
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }

        private void rollbackSql()
        {
            _transaction.Rollback();
        }
        #endregion

    }
}
