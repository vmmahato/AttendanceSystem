using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using AttendanceSystem.Database;
using AttendanceSystem.DatabaseConnectionFactory;

namespace AttendanceSystem.Services
{
    public class UnitOfWorkManager : IUnitOfWorkManager
    {
        private bool _isDisposed;
        private IDatabaseFactory _databaseFactory;
        public UnitOfWorkManager(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        /// <summary>
        /// Provides an instance of a unit of work. This wrapping in the manager
        /// class helps keep concerns separated
        /// 
        /// </summary>
        /// <returns></returns>
        public IDbContextTransaction NewUnitOfWork()
        {
            return _databaseFactory.Context.Database.BeginTransaction();
        }
        /// <summary>
        /// For SQL connection based unit of work
        /// To share with DbContext use NewUnitOfWork
        /// </summary>
        /// <returns></returns>
        public IUnitOfWork NewUnitOfWorkSql()
        {
            var unitOfWork = new UnitOfWork(_databaseFactory.Connection);
            _databaseFactory.ChangeTransaction(unitOfWork.Transaction);
            return unitOfWork;

        }
        /// <summary>
        /// Make sure there are no open sessions.
        /// In the web app this will be called when the injected UnitOfWork manager
        /// is disposed at the end of a request.
        /// </summary>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                //_context.Dispose();
                _isDisposed = true;
            }
        }
    }
}
